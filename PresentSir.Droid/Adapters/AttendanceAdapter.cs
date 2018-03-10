using Alansa.Droid.Adapters;
using Alansa.Droid.Collections;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using PresentSir.Droid.Models;

namespace PresentSir.Droid.Adapters
{
    internal class AttendanceAdapter : SmartAdapter<Attendance>
    {
        public AttendanceAdapter(ObservableCollection<Attendance> items, RecyclerView recyclerView) : base(items, recyclerView, Resource.Layout.row_attendance)
        {
        }

        protected override void OnLookupViewItems(View layout, GenericViewHolder viewHolder)
        {
            viewHolder.AddView("Name", layout.FindViewById<TextView>(Resource.Id.studentNameLbl));
            viewHolder.AddView("IndexNumber", layout.FindViewById<TextView>(Resource.Id.studentIdLbl));
        }

        protected override void OnUpdateView(GenericViewHolder holder, Attendance datum)
        {
            holder.GetView<TextView>("Name").Text = datum.Student.User.FullName;
            holder.GetView<TextView>("IndexNumber").Text = datum.Student.User.IndexNumber;
        }
    }
}