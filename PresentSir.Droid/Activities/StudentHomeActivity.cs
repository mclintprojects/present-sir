using Alansa.Droid.Activities;
using Alansa.Droid.Collections;
using Alansa.Droid.Utils;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using PresentSir.Droid.Adapters;
using PresentSir.Droid.Api;
using PresentSir.Droid.Dialogs;
using PresentSir.Droid.Models;
using System.Net;
using System.Threading.Tasks;
using EmptyStateManager = PresentSir.Droid.Utils.EmptyStateManager;
using IMenuItemOnMenuItemClickListener = Android.Support.V7.Widget.PopupMenu.IOnMenuItemClickListener;
using PopupMenu = Android.Support.V7.Widget.PopupMenu;

namespace PresentSir.Droid.Activities
{
    [Activity(Label = "Present, Sir")]
    public class StudentHomeActivity : BaseActivity, IMenuItemOnMenuItemClickListener
    {
        private View emptyState;
        private ProgressBar loadingCircle;
        private RecyclerView coursesRecycler;
        private int clickPos;
        private ObservableCollection<RegisteredCourse> collection;

        public override int LayoutResource => Resource.Layout.activity_student_home;

        protected override bool HomeAsUpEnabled => false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetupUI();
        }

        private void SetupUI()
        {
            emptyState = FindViewById(Resource.Id.empty);
            loadingCircle = FindViewById<ProgressBar>(Resource.Id.loadingCircle);

            coursesRecycler = FindViewById<RecyclerView>(Resource.Id.coursesRecycler);
            EmptyStateManager.SetEmptyState(emptyState, Resource.Drawable.ic_teacher_at_the_blackboard, "You haven't registered for any class");

            var registerForCourseFAB = FindViewById<FloatingActionButton>(Resource.Id.registerForCourseFAB);
            registerForCourseFAB.Click += delegate
            {
                var myself = PreferenceManager.Instance.GetJsonEntryAs<LoginResponse>("cred").Details;

                var dialog = new RegisterForClassDialog((int)myself["Id"]);
                dialog.OnStudentRegisterForClass += (s, registeredCourse) =>
                {
                    HideEmptyState();
                    collection.Add(registeredCourse);
                };

                dialog.Show(SupportFragmentManager, string.Empty);
            };

            LoadRegisteredCourses();
        }

        private async void LoadRegisteredCourses()
        {
            loadingCircle.Visibility = ViewStates.Visible;

            var currentUser = PreferenceManager.Instance.GetJsonEntryAs<LoginResponse>("cred").User;
            var response = await PresentSirApi.Instance.GetRegisteredCourses(currentUser.Id);

            if (response.Data != null)
            {
                HideEmptyState();

                collection = new ObservableCollection<RegisteredCourse>(response.Data);
                var adapter = new RegisteredCoursesAdapter(collection, coursesRecycler);
                coursesRecycler.SetLayoutManager(new LinearLayoutManager(this));
                coursesRecycler.SetAdapter(adapter);
                coursesRecycler.SetItemAnimator(new DefaultItemAnimator());

                if (collection.Count == 0)
                    ShowEmptyState();

                adapter.OnMoreClicked += Adapter_OnMoreClicked;
            }
            else
                Snackbar.Make(coursesRecycler, response.ErrorMessage, Snackbar.LengthIndefinite)
                    .SetAction("Retry", (v) => LoadRegisteredCourses())
                    .Show();

            loadingCircle.Visibility = ViewStates.Gone;
        }

        private void Adapter_OnMoreClicked(object sender, Alansa.Droid.Adapters.GenericViewHolder holder)
        {
            clickPos = holder.AdapterPosition;
            var anchor = holder.GetView<ImageView>("MoreVert");
            var popup = new PopupMenu(this, anchor);
            popup.MenuInflater.Inflate(Resource.Menu.popup_menu, popup.Menu);
            popup.SetOnMenuItemClickListener(this);
            popup.Show();
        }

        public bool OnMenuItemClick(IMenuItem item)
        {
            DeleteRegisteredCourse(collection[clickPos].Id);
            return true;
        }

        private async Task DeleteRegisteredCourse(int registeredCourseId)
        {
            loadingCircle.Visibility = ViewStates.Visible;

            var response = await PresentSirApi.Instance.DeleteRegisteredCourse(registeredCourseId);

            if (response.Data == HttpStatusCode.OK)
            {
                collection.RemoveAt(clickPos);

                if (collection.Count == 0)
                    ShowEmptyState();
            }
            else
                Snackbar.Make(coursesRecycler, "Something went wrong. Please retry.", Snackbar.LengthLong).Show();

            loadingCircle.Visibility = ViewStates.Gone;
        }

        private void ShowEmptyState()
        {
            coursesRecycler.Visibility = ViewStates.Gone;
            emptyState.Visibility = ViewStates.Visible;
        }

        private void HideEmptyState()
        {
            coursesRecycler.Visibility = ViewStates.Visible;
            emptyState.Visibility = ViewStates.Gone;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.student_home_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.logout:
                    App.Logout();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}