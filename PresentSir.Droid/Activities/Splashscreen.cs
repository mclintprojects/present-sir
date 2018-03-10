using Alansa.Droid.Utils;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using PresentSir.Droid.Models;

namespace PresentSir.Droid.Activities
{
    [Activity(Label = "Splashscreen", Theme = "@style/SplashTheme", NoHistory = true, MainLauncher = true)]
    public class Splashscreen : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var cred = PreferenceManager.Instance.GetJsonEntryAs<ApplicationUser>("cred");
            if (cred != null)
            {
                if (cred.AccountType == AccountType.Student)
                    StartActivity(new Intent(this, typeof(StudentHomeActivity)));
                else
                    StartActivity(new Intent(this, typeof(TeacherHomeActivity)));
            }
            else
                StartActivity(new Intent(this, typeof(LoginActivity)));
        }
    }
}