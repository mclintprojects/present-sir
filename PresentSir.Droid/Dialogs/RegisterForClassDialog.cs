using Alansa.Droid.Utils;
using Alansa.Droid.Views;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using PresentSir.Droid.Api;
using PresentSir.Droid.Models;
using System;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using DialogFragment = Android.Support.V4.App.DialogFragment;

namespace PresentSir.Droid.Dialogs
{
    internal class RegisterForClassDialog : DialogFragment
    {
        private readonly int studentId;

        public event EventHandler<RegisteredCourse> OnStudentRegisterForClass;

        public RegisterForClassDialog(int studentId)
        {
            this.studentId = studentId;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var view = LayoutInflater.From(Activity).Inflate(Resource.Layout.dialog_register_course, null, false);

            var classSearchView = view.FindViewById<ResourceSearchView>(Resource.Id.classSearchView);
            var registerForCourseBtn = view.FindViewById<Button>(Resource.Id.registerForCourseBtn);
            var loadingCircle = view.FindViewById<ProgressBar>(Resource.Id.loadingCircle);
            classSearchView.SetupSearch<Class>(PresentSirApi.Instance.GetClasses, null, Resource.Drawable.ic_teacher_at_the_blackboard);

            registerForCourseBtn.Click += async delegate
            {
                loadingCircle.Visibility = ViewStates.Visible;

                using (var validator = new Validator())
                {
                    validator.ValidateIsNotEmpty(classSearchView, true);

                    if (validator.PassedValidation)
                    {
                        var response = await PresentSirApi.Instance.RegisterForClass(studentId, classSearchView.SelectedItemId.Value);
                        if (response.Data != null)
                        {
                            OnStudentRegisterForClass?.Invoke(this, response.Data);
                            Dismiss();
                        }
                        else
                            Snackbar.Make(registerForCourseBtn, "Something went wrong. Please retry.", Snackbar.LengthLong).Show();
                    }
                }
            };

            return new AlertDialog.Builder(Activity)
                .SetTitle("Register for class")
                .SetView(view)
                .Create();
        }
    }
}