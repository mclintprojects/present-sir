package com.eyo.presentsir;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import org.json.JSONException;
import org.json.JSONObject;

public class LoginActivity extends AppCompatActivity implements SendData.AsyncResponse {

    EditText etUsername, etPassword;
    SendData asyncTask = null;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        etUsername = findViewById(R.id.etUsername);
        etPassword = findViewById(R.id.etPassword);
        asyncTask = new SendData(this);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_register, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }


    public void signupPage(View view) {
        Intent in = new Intent(LoginActivity.this, SignUpActivity.class);
        startActivity(in);
    }

    public void login(View view) {
        Log.i("Login activity", "Validating Data");

        if (etUsername.getText().toString().matches("") || etPassword.getText().toString().matches("")){
            Toast.makeText(LoginActivity.this, "Username and password are required", Toast.LENGTH_LONG).show();
        } else {
            Log.i("Login activity", "Data Validated");

            final String username = etUsername.getText().toString();
            final String password = etPassword.getText().toString();
            JSONObject loginJson = new JSONObject();
            try {
                loginJson.put("username", username);
                loginJson.put("password", password);

                asyncTask.execute("http://0470712f.ngrok.io/api/login?username="+username+"&password="+password);
                //new SendData().execute("http://0470712f.ngrok.io/api", loginJson.toString());

            } catch (JSONException ex) {
                Log.i("DoInbackground", "JSON exception: "+ex.getMessage());
            }
        }

    }


    @Override
    public void processFinish(String output) {

        try {
            JSONObject jsonObject = new JSONObject(output);
            Log.i("PostExecute", jsonObject.get("token").toString());
            Log.i("PostExecute", jsonObject.get("user").toString());
        } catch (JSONException ex) {

        }

    }
}
