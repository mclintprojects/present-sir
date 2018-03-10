using Alansa.Droid.Activities;
using Alansa.Droid.Extensions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using PresentSir.Droid.Api;
using System;
using System.Net;
using System.Text;

namespace PresentSir.Droid.Activities
{
    [Activity(Label = "Mark attendance")]
    public class MarkAttendanceActivity : BaseActivity
    {
        private int classId;

        public override int LayoutResource => Resource.Layout.activity_mark_attendance;
        private bool initiatedMarking, endedMarking;
        private ProgressBar loadingCircle;
        private Button finishMarkingBtn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            classId = Intent.GetIntExtra("classId", 0);
            ShowAttendanceMarkingGuideDialog(classId);

            FindViewById<TextView>(Resource.Id.ussdCodeTextView).Text = $"{AppResx.ServiceCode}*{{STUDENT_INDEX_NUMBER_HERE}}*{classId}";

            loadingCircle = FindViewById<ProgressBar>(Resource.Id.loadingCircle);
            finishMarkingBtn = FindViewById<Button>(Resource.Id.finishMarking);
            finishMarkingBtn.Click += FinishMarkingBtn_Click;
        }

        private void FinishMarkingBtn_Click(object sender, EventArgs e)
        {
            if (!initiatedMarking)
            {
                InitiateMarking();
                initiatedMarking = true;
            }
            else
            {
                FinishMarking();
                endedMarking = true;
                NavigateAway();
            }
        }

        public override void NavigateAway()
        {
            if (endedMarking)
                base.NavigateAway();
        }

        private async void FinishMarking()
        {
            loadingCircle.Visibility = ViewStates.Visible;

            var response = await PresentSirApi.Instance.ManageSession("start", classId);
            if (response.Data == HttpStatusCode.OK)
            {
                finishMarkingBtn.Text = "Finish";
                this.ShowPositiveDialog("Session started", "This marking sesssion has been successfully started.");
            }
            else
                this.ShowPositiveDialog("Session error", "Something went wrong starting this session.");

            loadingCircle.Visibility = ViewStates.Gone;
        }

        private async void InitiateMarking()
        {
            loadingCircle.Visibility = ViewStates.Visible;

            var response = await PresentSirApi.Instance.ManageSession("finish", classId);
            if (response.Data == HttpStatusCode.OK)
            {
                finishMarkingBtn.Text = "Start";
                this.ShowPositiveDialog("Session ended", "This marking sesssion has been successfully ended.");
            }
            else
                this.ShowPositiveDialog("Session error", "Something went wrong ending this session.");

            loadingCircle.Visibility = ViewStates.Gone;
        }

        private void ShowAttendanceMarkingGuideDialog(int classId)
        {
            var message = new StringBuilder();

            message.AppendLine("Please write the following attendance code on a board for all registered students to mark their attendance.")
            .AppendLine()
            .AppendLine($"{AppResx.ServiceCode}*{{STUDENT_INDEX_NUMBER_HERE}}*{classId}");

            this.ShowPositiveDialog("Mark attendance", message.ToString());
        }
    }
}