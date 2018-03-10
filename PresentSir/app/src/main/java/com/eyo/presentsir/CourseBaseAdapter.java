package com.eyo.presentsir;

import android.content.Context;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.CheckBox;

import java.util.ArrayList;

/**
 * Created by eyo on 3/10/2018.
 * ${CLASS_NAME}
 */

public class CourseBaseAdapter extends BaseAdapter {
    private ArrayList<CourseViewItem> courses;
    private Context mContext = null;

    public CourseBaseAdapter(Context context, ArrayList<CourseViewItem> courses) {
        this.courses = courses;
        mContext = context;
    }

    @Override
    public int getCount() {
        return courses.size();
    }

    @Override
    public Object getItem(int position) {
        return courses.get(position);
    }

    @Override
    public long getItemId(int position) {
        return position;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        CourseViewHolder viewHolder = null;

        convertView = View.inflate(mContext, R.layout.course_list_view, null);
        CheckBox courseCheckbox = convertView.findViewById(R.id.cbCourseItem);
        viewHolder = new CourseViewHolder(convertView);

        viewHolder.setCourseCheckbox(courseCheckbox);
        convertView.setTag(viewHolder);
        CourseViewItem courseViewItem = courses.get(position);
        viewHolder.getCourseCheckbox().setChecked(courseViewItem.isChecked());
        viewHolder.getCourseCheckbox().setText(courseViewItem.getCourseName());

        return convertView;
    }
}
