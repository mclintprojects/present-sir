package com.eyo.presentsir;

import android.support.v7.widget.RecyclerView;
import android.view.View;
import android.widget.CheckBox;

/**
 * Created by eyo on 3/10/2018.
 * ${CLASS_NAME}
 */

public class CourseViewHolder extends RecyclerView.ViewHolder {
    private CheckBox courseCheckbox;

    public CourseViewHolder(View itemView) {
        super(itemView);
    }

    public CheckBox getCourseCheckbox() {
        return courseCheckbox;
    }

    public void setCourseCheckbox(CheckBox courseCheckbox) {
        this.courseCheckbox = courseCheckbox;
    }
}
