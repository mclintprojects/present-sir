using Alansa.Droid.Activities;
using Alansa.Droid.Extensions;
using Alansa.Droid.Utils;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using PresentSir.Droid.Api;
using PresentSir.Droid.Dialogs;
using PresentSir.Droid.Models;
using System;

namespace PresentSir.Droid.Activities
{
    [Activity(Label = "Login")]
    public class LoginActivity : BaseActivity
    {
        private EditText etUsername;
        private EditText etPassword;
        private ProgressBar loadingCircle;

        public override int LayoutResource => Resource.Layout.activity_login;

        protected override bool HomeAsUpEnabled => false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetupUI();
        }

        private void SetupUI()
        {
            var noAccountLbl = FindViewById<TextView>(Resource.Id.tvNoAccount);
            var signupBtnsRoot = FindViewById<LinearLayout>(Resource.Id.signupBtnsRoot);
            noAccountLbl.Click += delegate
            {
                signupBtnsRoot.Visibility = ViewStates.Visible;
            };

            loadingCircle = FindViewById<ProgressBar>(Resource.Id.loadingCircle);
            etUsername = FindViewById<EditText>(Resource.Id.etUsername);
            etPassword = FindViewById<EditText>(Resource.Id.etPassword);
            var loginBtn = FindViewById<Button>(Resource.Id.btnLogin);

            loginBtn.Click += LoginBtn_Click;

            var teacherSignupBtn = FindViewById<Button>(Resource.Id.teacherSignUpBtn);
            var studentSignupBtn = FindViewById<Button>(Resource.Id.studentSignUpBtn);

            teacherSignupBtn.Click += TeacherSignupBtn_Click;
            studentSignupBtn.Click += StudentSignupBtn_Click;
        }

        private void StudentSignupBtn_Click(object sender, EventArgs e)
        {
            var dialog = new StudentSignUpDialog();
            dialog.OnStudentRegistered += delegate
            {
                Toast.MakeText(this, "You have successfully signed up. Please login.", ToastLength.Long).Show();
            };
            dialog.Show(SupportFragmentManager, string.Empty);
        }

        private void TeacherSignupBtn_Click(object sender, EventArgs e)
        {
            var dialog = new TeacherSignUpDialog();
            dialog.OnTeacherRegistered += delegate
            {
                Toast.MakeText(this, "You have successfully signed up. Please login.", ToastLength.Long).Show();
            };
            dialog.Show(SupportFragmentManager, string.Empty);
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            loadingCircle.Visibility = ViewStates.Visible;

            using (var validator = new Validator())
            {
                validator.ValidateIsNotEmpty(etUsername, true);
                validator.ValidateIsNotEmpty(etPassword, true);

                if (validator.PassedValidation)
                {
                    LoginUser(etUsername.Text, etPassword.Text);
                }
            }
        }

        private async void LoginUser(string username, string password)
        {
            var response = await PresentSirApi.Instance.LoginAsync(username, password);
            if (response.Data != null)
            {
                PreferenceManager.Instance.AddJsonEntry("cred", response.Data.ToJson());
                PresentSirApi.Instance.AddAuthorizationToken(response.Data.Token);

                if (response.Data.User.AccountType == AccountType.Student)
                    StartActivity(new Intent(this, typeof(StudentHomeActivity)));
                else
                    StartActivity(new Intent(this, typeof(TeacherHomeActivity)));
            }
            else
                this.ShowPositiveDialog("Error", response.ErrorMessage);

            loadingCircle.Visibility = ViewStates.Gone;
        }
    }
}