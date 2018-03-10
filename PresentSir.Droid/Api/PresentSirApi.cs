using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using PresentSir.Droid.Models;
using PresentSir.Droid.Extensions;

namespace PresentSir.Droid.Api
{
    internal class PresentSirApi
    {
        private readonly HttpClient client;
        private string root = AppResx.ApiRoot;
        private static PresentSirApi singleton;
        private readonly JsonSerializer serializer = new JsonSerializer();

        public static PresentSirApi Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new PresentSirApi();

                return singleton;
            }
        }

        private string token;
        public string Token => token;

        private PresentSirApi()
        {
            if (client == null)
                client = new HttpClient();

            client.Timeout = TimeSpan.FromSeconds(20);
        }

        private T ResponseFromStream<T>(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                using (var json = new JsonTextReader(reader))
                    return serializer.Deserialize<T>(json);
            }
        }

        public void AddAuthorizationToken(string token)
        {
            this.token = token;
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        public async Task<ApiResponse<LoginResponse>> LoginAsync(string username, string password)
        {
            try
            {
                var response = await client.PostAsync($"{root}/login?username={username}&password={password}", null);
                var loginResponse = ResponseFromStream<LoginResponse>(await response.Content.ReadAsStreamAsync());

                if (!string.IsNullOrEmpty(loginResponse.Token))
                    return new ApiResponse<LoginResponse>(loginResponse, string.Empty);
                else
                    return new ApiResponse<LoginResponse>(null, await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                return new ApiResponse<LoginResponse>(null, e.GetExceptionMessage());
            }
        }
    }
}