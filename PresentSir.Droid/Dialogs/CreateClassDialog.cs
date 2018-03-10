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
    internal class CreateClassDialog : DialogFragment
    {
        private readonly int teacherId;

        public event EventHandler<Class> OnteacherCreateClass;

        public CreateClassDialog(int teacherId)
        {
            this.teacherId = teacherId;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var view = LayoutInflater.From(Activity).Inflate(Resource.Layout.dialog_create_course, null, false);

            var institutionSearchView = view.FindViewById<ResourceSearchView>(Resource.Id.institutionSearchView);
            var courseCodeTb = view.FindViewById<EditText>(Resource.Id.courseCodeTb);
            var createCourseBtn = view.FindViewById<Button>(Resource.Id.createCourseBtn);
            var loadingCircle = view.FindViewById<ProgressBar>(Resource.Id.loadingCircle);
            institutionSearchView.SetupSearch<Institution>(PresentSirApi.Instance.GetInstitutions, null, Resource.Drawable.ic_teacher_at_the_blackboard);

            createCourseBtn.Click += async delegate
            {
                loadingCircle.Visibility = ViewStates.Visible;

                using (var validator = new Validator())
                {
                    validator.ValidateIsNotEmpty(institutionSearchView, true);
                    validator.ValidateIsNotEmpty(courseCodeTb, true);

                    if (validator.PassedValidation)
                    {
                        var newClass = new Class
                        {
                            CourseCode = courseCodeTb.Text,
                            InstitutionId = institutionSearchView.SelectedItemId.Value,
                            TeacherId = teacherId
                        };

                        var response = await PresentSirApi.Instance.CreateClass(newClass);
                        if (response.Data != null)
                        {
                            OnteacherCreateClass?.Invoke(this, response.Data);
                            Dismiss();
                        }
                        else
                            Snackbar.Make(createCourseBtn, "Something went wrong. Please retry.", Snackbar.LengthLong).Show();
                    };
                }
            };

            return new AlertDialog.Builder(Activity).
                SetTitle("Create a class")
                .SetView(view)
                .Create();
        }
    }
}