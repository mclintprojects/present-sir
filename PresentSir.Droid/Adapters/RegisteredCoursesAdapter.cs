using Alansa.Droid.Adapters;
using Alansa.Droid.Collections;
using Alansa.Droid.Extensions;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using PresentSir.Droid.Models;
using System;

namespace PresentSir.Droid.Adapters
{
    internal class RegisteredCoursesAdapter : SmartAdapter<RegisteredCourse>
    {
        public event EventHandler<GenericViewHolder> OnMoreClicked;

        public RegisteredCoursesAdapter(ObservableCollection<RegisteredCourse> collection, RecyclerView recycler) : base(collection, recycler, Resource.Layout.row_registered_course)
        {
        }

        protected override void OnLookupViewItems(View layout, GenericViewHolder viewHolder)
        {
            viewHolder.AddView("Avi", layout.FindViewById<TextView>(Resource.Id.Avi));
            viewHolder.AddView("CourseCode", layout.FindViewById<TextView>(Resource.Id.Name));
            viewHolder.AddView("Institution", layout.FindViewById<TextView>(Resource.Id.Description));
            viewHolder.AddView("MoreVert", layout.FindViewById<ImageView>(Resource.Id.moreVert));

            layout.FindViewById<ImageView>(Resource.Id.moreVert).Click += delegate
            {
                OnMoreClicked?.Invoke(this, viewHolder);
            };
        }

        protected override void OnUpdateView(GenericViewHolder holder, RegisteredCourse datum)
        {
            holder.GetView<TextView>("Avi").Text = datum.Class.CourseCode.GetAcronym();
            holder.GetView<TextView>("CourseCode").Text = datum.Class.CourseCode;
            holder.GetView<TextView>("Institution").Text = datum.Class.Institution.Name;
        }
    }
}