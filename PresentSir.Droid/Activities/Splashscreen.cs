using Alansa.Droid.Utils;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using PresentSir.Droid.Api;
using PresentSir.Droid.Models;

namespace PresentSir.Droid.Activities
{
    [Activity(Label = "Present, Sir", Theme = "@style/SplashTheme", NoHistory = true, MainLauncher = true)]
    public class Splashscreen : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var cred = PreferenceManager.Instance.GetJsonEntryAs<LoginResponse>("cred");
            if (cred != null)
            {
                PresentSirApi.Instance.AddAuthorizationToken(cred.Token);

                if (cred.User.AccountType == AccountType.Student)
                    StartActivity(new Intent(this, typeof(StudentHomeActivity)));
                else
                    StartActivity(new Intent(this, typeof(TeacherHomeActivity)));
            }
            else
                StartActivity(new Intent(this, typeof(LoginActivity)));
        }
    }
}