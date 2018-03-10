using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Alansa.Droid.Activities;
using Alansa.Droid.Utils;
using PresentSir.Droid.Api;
using Alansa.Droid.Extensions;
using PresentSir.Droid.Models;

namespace PresentSir.Droid.Activities
{
    [Activity(Label = "Login")]
    public class LoginActivity : BaseActivity
    {
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

            var etUsername = FindViewById<EditText>(Resource.Id.etUsername);
            var etPassword = FindViewById<EditText>(Resource.Id.etPassword);
            var loginBtn = FindViewById<Button>(Resource.Id.btnLogin);

            loginBtn.Click += LoginBtn_Click;

            var teacherSignupBtn = FindViewById<Button>(Resource.Id.teacherSignUpBtn);
            var studentSignupBtn = FindViewById<Button>(Resource.Id.studentSignUpBtn);

            teacherSignupBtn.Click
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            using (var validator = new Validator())
            {
                validator.ValidateIsNotEmpty(etUsername, true);
                validator.ValidateIsNotEmpty(etPassword);

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
        }
    }
}