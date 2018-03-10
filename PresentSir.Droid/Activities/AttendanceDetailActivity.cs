using Alansa.Droid.Activities;
using Alansa.Droid.Collections;
using Alansa.Droid.Extensions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using PresentSir.Droid.Adapters;
using PresentSir.Droid.Api;
using PresentSir.Droid.Models;
using PresentSir.Droid.Utils;
using System;

namespace PresentSir.Droid.Activities
{
    [Activity(Label = "Attendance details")]
    public class AttendanceDetailActivity : BaseActivity
    {
        private int classId;
        private ProgressBar loadingCircle;
        private RecyclerView recyclerView;
        private TextView dateLbl;
        private DateTime selectedDate;
        private View emptyState;

        public override int LayoutResource => Resource.Layout.activity_attendance_details;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            loadingCircle = FindViewById<ProgressBar>(Resource.Id.loadingCircle);
            recyclerView = FindViewById<RecyclerView>(Resource.Id.attendanceRecyclerView);
            dateLbl = FindViewById<TextView>(Resource.Id.attendanceDateLbl);
            emptyState = FindViewById(Resource.Id.empty);
            classId = Intent.GetIntExtra("classId", 0);

            EmptyStateManager.SetEmptyState(emptyState, Resource.Drawable.ic_female_graduate_student, "No student attended class on this day.");

            SetupUI();
        }

        private void SetupUI()
        {
            SetupDateLabel();
            SetupAttendanceList();
        }

        private async void SetupAttendanceList()
        {
            loadingCircle.Visibility = ViewStates.Visible;

            var response = await PresentSirApi.Instance.GetAttendance(int.MaxValue, 1, classId, $"{selectedDate.Day}/{selectedDate.Month}/{selectedDate.Year}");
            if (response.Data?.Data != null)
            {
                var collection = new ObservableCollection<Attendance>(response.Data.Data);
                var adapter = new AttendanceAdapter(collection, recyclerView);
                recyclerView.SetLayoutManager(new LinearLayoutManager(this));
                recyclerView.SetAdapter(adapter);
                recyclerView.SetItemAnimator(new DefaultItemAnimator());

                if (collection.Count == 0)
                    ShowEmptyState();
            }

            loadingCircle.Visibility = ViewStates.Gone;
        }

        private void ShowEmptyState()
        {
            emptyState.Visibility = ViewStates.Visible;
            recyclerView.Visibility = ViewStates.Gone;
        }

        private void HideEmptyState()
        {
            emptyState.Visibility = ViewStates.Gone;
            recyclerView.Visibility = ViewStates.Visible;
        }

        private void SetupDateLabel()
        {
            dateLbl.Text = DateTime.Now.ToPrettyDate();

            dateLbl.Click += delegate
            {
                var dialog = new DatePickerDialog(this, (sender, e) =>
                {
                    dateLbl.Text = e.Date.ToPrettyDate();
                    selectedDate = e.Date;
                }, DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day);
                dialog.Show();
            };
        }
    }
}