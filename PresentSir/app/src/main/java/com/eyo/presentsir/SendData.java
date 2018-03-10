package com.eyo.presentsir;

import android.os.AsyncTask;
import android.util.Log;

import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;

/**
 * Created by eyo on 3/9/2018.
 * ${CLASS_NAME}
 */

public class SendData extends AsyncTask<String, Void, String> {

    public interface AsyncResponse {
        void processFinish(String output);
    }

    public AsyncResponse delegate = null;

    public SendData(AsyncResponse delegate){
        this.delegate = delegate;
    }

    @Override
    protected String doInBackground(String... strings) {
        String data = "";

        HttpURLConnection urlConnection = null;
        try {
            Log.i("DoInbackground", "About to send post");
            urlConnection = (HttpURLConnection) new URL(strings[0]).openConnection();
            urlConnection.setRequestMethod("POST");
            urlConnection.setDoOutput(true);
            urlConnection.setDoInput(true);


            DataOutputStream wr = new DataOutputStream(urlConnection.getOutputStream());
            wr.writeBytes("PostData=" + strings[0]);
            wr.flush();
            wr.close();

            Log.i("DoInbackground", "StatusCode: "+urlConnection.getResponseCode());

            InputStream in = urlConnection.getInputStream();
            InputStreamReader inputStreamReader = new InputStreamReader(in);

            int inputStreamData = inputStreamReader.read();
            while (inputStreamData != -1) {
                char current = (char) inputStreamData;
                inputStreamData = inputStreamReader.read();
                data += current;
            }


        } catch (MalformedURLException ex) {
            Log.e("DoInbackgorund", "Exception occurred: "+ex.toString());
        }catch (IOException ex) {
            Log.e("DoInbackgorund", "Exception occurred: "+ex.toString());
        }catch (Exception ex) {
                Log.e("DoInbackgorund", "Exception occurred: "+ex.toString());
        } finally {
            if (urlConnection != null) {
                urlConnection.disconnect();
            }
        }

        return data;
    }

    @Override
    protected void onPreExecute() {
        super.onPreExecute();
    }

    @Override
    protected void onPostExecute(String result) {
        super.onPostExecute(result);
        Log.i("AsyncPostExecute", result);
        delegate.processFinish(result);

    }
}
