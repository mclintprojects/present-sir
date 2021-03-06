using Alansa.Droid.Extensions;
using Alansa.Droid.Utils;
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
    internal class TeacherSignUpDialog : DialogFragment
    {
        private EditText retypePwdTb;
        private EditText usernameTb;
        private EditText fullnameTb;
        private EditText passwordTb;
        private ProgressBar loadingCircle;

        public event EventHandler OnTeacherRegistered;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var view = LayoutInflater.From(Activity).Inflate(Resource.Layout.dialog_sign_up, null, false);

            view.FindViewById<TextInputLayout>(Resource.Id.indexNoTIL).Visibility = ViewStates.Gone;

            fullnameTb = view.FindViewById<EditText>(Resource.Id.etSignUpFullname);
            usernameTb = view.FindViewById<EditText>(Resource.Id.etsignUpUsername);
            passwordTb = view.FindViewById<EditText>(Resource.Id.etSignUpPassword);
            retypePwdTb = view.FindViewById<EditText>(Resource.Id.etSignUpPassword2);
            loadingCircle = view.FindViewById<ProgressBar>(Resource.Id.loadingCircle);

            var signupBtn = view.FindViewById<Button>(Resource.Id.signupBtn);
            signupBtn.Click += SignupBtn_Click;

            return new AlertDialog.Builder(Activity)
                .SetTitle("Teacher sign-up")
                .SetView(view)
                .Create();
        }

        private async void SignupBtn_Click(object sender, EventArgs e)
        {
            loadingCircle.Visibility = ViewStates.Visible;

            using (var validator = new Validator())
            {
                validator.ValidateIsNotEmpty(fullnameTb, true);
                validator.ValidateIsNotEmpty(usernameTb, true);
                validator.ValidateIsSame(passwordTb, retypePwdTb);

                if (validator.PassedValidation)
                {
                    var newUser = new ApplicationUser
                    {
                        AccountType = AccountType.Teacher,
                        FullName = fullnameTb.Text.Trim(),
                        Password = passwordTb.Text.Trim(),
                        Username = usernameTb.Text.Trim()
                    };

                    var response = await PresentSirApi.Instance.RegisterAsync(newUser);
                    if (response.Data != null)
                    {
                        OnTeacherRegistered?.Invoke(this, EventArgs.Empty);
                        Dismiss();
                    }
                    else
                        Activity.ShowPositiveDialog("Error", response.ErrorMessage);
                }
            }

            loadingCircle.Visibility = ViewStates.Gone;
        }
    }
}