package com.eyo.presentsir;

import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ListView;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;

public class StudentHomeActivity extends AppCompatActivity implements SendData.AsyncResponse{

    ArrayList<CourseViewItem> courses;
    ListView lvCourses;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_student_home);

        new SendData(this).execute("getCourses?");

        lvCourses = findViewById(R.id.lvCourses);

        final CourseBaseAdapter adapter = new CourseBaseAdapter(this, courses);

        adapter.notifyDataSetChanged();
        lvCourses.setAdapter(adapter);

        lvCourses.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {

            }
        });

    }

    @Override
    public void processFinish(String output) {
        try {
            JSONObject result = new JSONObject(output);
            courses = new ArrayList<>();
            /*for (result.get("courses"): course) {
                courses.add(course);

            }*/
        } catch (JSONException ex) {
            ex.printStackTrace();
        }

    }
}
