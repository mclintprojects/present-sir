package com.eyo.presentsir;

import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;

import java.util.ArrayList;

public class StudentHomeActivity extends AppCompatActivity implements SendData.AsyncResponse{

    ArrayList<String> courses;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_student_home);

        new SendData(this).execute("getCourses?");

    }

    @Override
    public void processFinish(String output) {

    }
}
