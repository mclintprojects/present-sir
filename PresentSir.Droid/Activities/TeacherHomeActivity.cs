using Alansa.Droid.Activities;
using Alansa.Droid.Collections;
using Alansa.Droid.Utils;
using Android.App;
using Android.Content;
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
    public class TeacherHomeActivity : BaseActivity, IMenuItemOnMenuItemClickListener
    {
        private ProgressBar loadingCircle;
        private View emptyState;
        private RecyclerView coursesRecycler;
        private ObservableCollection<Class> collection;
        private int clickPos;

        public override int LayoutResource => Resource.Layout.activity_teacher_home;
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
            EmptyStateManager.SetEmptyState(emptyState, Resource.Drawable.ic_teacher_at_the_blackboard, "You haven't created any class");

            var createCourseFAB = FindViewById<FloatingActionButton>(Resource.Id.createCourseFAB);
            createCourseFAB.Click += delegate
            {
                var myself = PreferenceManager.Instance.GetJsonEntryAs<LoginResponse>("cred").Details;

                var dialog = new CreateClassDialog((int)myself["Id"]);
                dialog.OnteacherCreateClass += (sender, @class) =>
                {
                    HideEmptyState();
                    collection.Add(@class);
                    Toast.MakeText(this, "Class created successfully.", ToastLength.Long).Show();
                };
                dialog.Show(SupportFragmentManager, string.Empty);
            };

            LoadCourses();
        }

        private async void LoadCourses()
        {
            loadingCircle.Visibility = ViewStates.Visible;

            var details = PreferenceManager.Instance.GetJsonEntryAs<LoginResponse>("cred").Details;
            var teacherId = (int)details["Id"];

            var response = await PresentSirApi.Instance.GetTeacherCourses(30, 1, teacherId);

            if (response.Data?.Data != null)
            {
                HideEmptyState();

                collection = new ObservableCollection<Class>(response.Data.Data);
                var adapter = new TeacherCoursesAdapter(collection, coursesRecycler);
                coursesRecycler.SetLayoutManager(new LinearLayoutManager(this));
                coursesRecycler.SetAdapter(adapter);
                coursesRecycler.SetItemAnimator(new DefaultItemAnimator());

                adapter.ItemClicked += (pos) =>
                {
                    var intent = new Intent(this, typeof(AttendanceDetailActivity));
                    intent.PutExtra("classId", collection[pos].Id);
                    StartActivity(intent);
                };

                if (collection.Count == 0)
                    ShowEmptyState();

                adapter.OnMoreClicked += Adapter_OnMoreClicked;
            }
            else
                Snackbar.Make(coursesRecycler, response.ErrorMessage, Snackbar.LengthIndefinite)
                    .SetAction("Retry", (v) => LoadCourses())
                    .Show();

            loadingCircle.Visibility = ViewStates.Gone;
        }

        private void Adapter_OnMoreClicked(object sender, Alansa.Droid.Adapters.GenericViewHolder holder)
        {
            clickPos = holder.AdapterPosition;
            var anchor = holder.GetView<ImageView>("MoreVert");
            var popup = new PopupMenu(this, anchor);
            popup.MenuInflater.Inflate(Resource.Menu.course_list_popup_menu, popup.Menu);
            popup.SetOnMenuItemClickListener(this);
            popup.Show();
        }

        public bool OnMenuItemClick(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.deleteItem:
                    DeleteCourse(collection[clickPos].Id);
                    return true;

                case Resource.Id.markAttendance:
                    var intent = new Intent(this, typeof(MarkAttendanceActivity));
                    intent.PutExtra("classId", collection[clickPos].Id);
                    StartActivity(intent);
                    return true;
            }

            return false;
        }

        private async Task DeleteCourse(int courseId)
        {
            loadingCircle.Visibility = ViewStates.Visible;

            var response = await PresentSirApi.Instance.DeleteTeacherCourse(courseId);

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