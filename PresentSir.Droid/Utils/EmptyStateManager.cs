using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace PresentSir.Droid.Utils
{
    public static class EmptyStateManager
    {
        public static void SetEmptyState(View emptyState, int iconResId, string emptyStateLabelText)
        {
            emptyState.FindViewById<TextView>(Resource.Id.stateText).Text = emptyStateLabelText;
            emptyState.FindViewById<ImageView>(Resource.Id.emptyIcon).SetImageDrawable(AppCompatDrawableManager.Get().GetDrawable(App.CurrentActivity, iconResId));
        }
    }
}