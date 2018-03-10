package com.eyo.presentsir;

import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.Toast;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;

public class SignUpActivity extends AppCompatActivity {

    EditText etSignUpFullname, etSignUpUsername, etSignUpPassword, etSignUpPassword2, etSignUpIndexNumber;
    Spinner spCategory;
    Button btnRegister;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_sign_up);

        etSignUpFullname = findViewById(R.id.etSignUpFullname);
        etSignUpUsername = findViewById(R.id.etsignUpUsername);
        etSignUpPassword = findViewById(R.id.etSignUpPassword);
        etSignUpPassword2 = findViewById(R.id.etSignUpPassword2);
        etSignUpIndexNumber = findViewById(R.id.etSignUpIndexNumber);
        etSignUpFullname = findViewById(R.id.etSignUpFullname);

        spCategory = findViewById(R.id.spCategory);

        ArrayList<String> categoryList = new ArrayList<>(2);
        categoryList.add("Select....");
        categoryList.add("Teacher");
        categoryList.add("Student");

        ArrayAdapter adapter = new ArrayAdapter(getApplicationContext(), android.R.layout.simple_spinner_dropdown_item, categoryList);
        spCategory.setAdapter(adapter);


    }

    public void register(View view) {
        Log.i("Register Info", "Validating data");

        if (etSignUpUsername.getText().toString().matches("") ||
                etSignUpPassword.getText().toString().matches("") ||
                etSignUpPassword2.getText().toString().matches("") ||
                etSignUpFullname.getText().toString().matches("")
                ){
            Toast.makeText(SignUpActivity.this,
                    "All fields are required",
                    Toast.LENGTH_LONG).show();
        } else if (!etSignUpPassword.getText().toString().matches(etSignUpPassword2.getText().toString())){
            Toast.makeText(SignUpActivity.this,
                    "Passwords do not match",
                    Toast.LENGTH_LONG).show();
        } else if (spCategory.getSelectedItem().toString().matches("Select...")){
            Toast.makeText(SignUpActivity.this,
                    "Not Selected",
                    Toast.LENGTH_LONG).show();
        } else if (spCategory.getSelectedItem().toString().matches("Student") &&
                etSignUpIndexNumber.getText().toString().matches("")){
            Toast.makeText(SignUpActivity.this,
                    "Index Number Required for Students",
                    Toast.LENGTH_LONG).show();
        } else {
            Log.i("Login activity", "Data Validated");
            int category = 0;
            if (spCategory.getSelectedItem().toString().matches("Teacher")) {
                category = 1;
            } else {
                category = 2;
            }
            final String fullname = etSignUpFullname.getText().toString();
            final String username = etSignUpUsername.getText().toString();
            final String password = etSignUpPassword.getText().toString();
            final String indexNumber = etSignUpIndexNumber.getText().toString();
            JSONObject signupJson = new JSONObject();
            try {
                signupJson.put("fullname", fullname);
                signupJson.put("username", username);
                signupJson.put("password", password);
                signupJson.put("indexNumber", indexNumber);
                signupJson.put("AccountType", category);

                new SendData().execute("http://0470712f.ngrok.io/api/register", signupJson.toString());

            } catch (JSONException ex) {

            }

        }

    }


}
