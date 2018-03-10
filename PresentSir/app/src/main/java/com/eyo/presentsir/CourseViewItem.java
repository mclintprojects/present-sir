package com.eyo.presentsir;

/**
 * Created by eyo on 3/10/2018.
 * ${CLASS_NAME}
 */

public class CourseViewItem {
    private boolean checked = false;
    private String courseName = "";

    public boolean isChecked() {
        return checked;
    }

    public void setChecked(boolean checked) {
        this.checked = checked;
    }

    public String getCourseName() {
        return courseName;
    }

    public void setCourseName(String courseName) {
        this.courseName = courseName;
    }
}
