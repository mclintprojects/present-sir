using Alansa.Droid.Utils;
using Android.App;
using Android.Content;
using Android.Runtime;
using PresentSir.Droid.Activities;
using PresentSir.Droid.Api;
using System;

namespace PresentSir.Droid
{
    [Application]
    internal class App : Alansa.Droid.App
    {
        public App(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public static void Logout()
        {
            PreferenceManager.Instance.AddJsonEntry("cred", null);
            PresentSirApi.Instance.RemoveAuthorization();
            var intent = new Intent(CurrentActivity, typeof(LoginActivity));
            intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
            CurrentActivity.StartActivity(intent);
        }
    }
}