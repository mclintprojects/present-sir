using Alansa.Droid.Extensions;
using Alansa.Droid.Interfaces;
using Newtonsoft.Json;
using PresentSir.Droid.Extensions;
using PresentSir.Droid.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

        internal void RemoveAuthorization()
        {
            this.token = null;
            client.DefaultRequestHeaders.Remove("Authorization");
        }

        public async Task<ApiResponse<LoginResponse>> LoginAsync(string username, string password)
        {
            try
            {
                var response = await client.PostAsync($"{root}/login?username={username}&password={password}", null);

                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = ResponseFromStream<LoginResponse>(await response.Content.ReadAsStreamAsync());
                    return new ApiResponse<LoginResponse>(loginResponse, string.Empty);
                }
                else
                    return new ApiResponse<LoginResponse>(null, await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                return new ApiResponse<LoginResponse>(null, e.GetExceptionMessage());
            }
        }

        public async Task<ApiResponse<ApplicationUser>> RegisterAsync(ApplicationUser user)
        {
            try
            {
                var response = await client.PostAsync($"{root}/register", new StringContent(user.ToJson(), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var newUser = ResponseFromStream<ApplicationUser>(await response.Content.ReadAsStreamAsync());
                    return new ApiResponse<ApplicationUser>(newUser, string.Empty);
                }
                else
                    return new ApiResponse<ApplicationUser>(null, await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                return new ApiResponse<ApplicationUser>(null, e.GetExceptionMessage());
            }
        }

        public async Task<ApiResponse<Class>> CreateClass(Class @class)
        {
            try
            {
                var response = await client.PostAsync($"{root}/classes", new StringContent(@class.ToJson(), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var newClass = ResponseFromStream<Class>(await response.Content.ReadAsStreamAsync());
                    return new ApiResponse<Class>(newClass, string.Empty);
                }
                else
                    return new ApiResponse<Class>(null, await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                return new ApiResponse<Class>(null, e.GetExceptionMessage());
            }
        }

        public async Task<ApiResponse<List<RegisteredCourse>>> GetRegisteredCourses(int userId)
        {
            try
            {
                var response = await client.GetAsync($"{root}/user/courses/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    var registeredCourses = ResponseFromStream<List<RegisteredCourse>>(await response.Content.ReadAsStreamAsync());
                    return new ApiResponse<List<RegisteredCourse>>(registeredCourses, string.Empty);
                }
                else
                    return new ApiResponse<List<RegisteredCourse>>(null, await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                return new ApiResponse<List<RegisteredCourse>>(null, e.GetExceptionMessage());
            }
        }

        public async Task<ApiResponse<GetTeacherCoursesResponse>> GetTeacherCourses(int perPage, int pageNumber, int teacherId)
        {
            try
            {
                var response = await client.GetAsync($"{root}/classes?perPage={perPage}&pageNumber={pageNumber}&teacherId={teacherId}");

                if (response.IsSuccessStatusCode)
                {
                    var registeredCourses = ResponseFromStream<GetTeacherCoursesResponse>(await response.Content.ReadAsStreamAsync());
                    return new ApiResponse<GetTeacherCoursesResponse>(registeredCourses, string.Empty);
                }
                else
                    return new ApiResponse<GetTeacherCoursesResponse>(null, await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                return new ApiResponse<GetTeacherCoursesResponse>(null, e.GetExceptionMessage());
            }
        }

        public async Task<ApiResponse<RegisteredCourse>> RegisterForClass(int studentId, int classId)
        {
            try
            {
                var response = await client.PostAsync($"{root}/classes/register/{studentId}/{classId}", null);

                if (response.IsSuccessStatusCode)
                {
                    var newRegistration = ResponseFromStream<RegisteredCourse>(await response.Content.ReadAsStreamAsync());
                    return new ApiResponse<RegisteredCourse>(newRegistration, string.Empty);
                }
                else
                    return new ApiResponse<RegisteredCourse>(null, await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                return new ApiResponse<RegisteredCourse>(null, e.GetExceptionMessage());
            }
        }

        public async Task<ApiResponse<HttpStatusCode>> DeleteRegisteredCourse(int courseId)
        {
            try
            {
                var response = await client.DeleteAsync($"{root}/registeredcourse?courseId={courseId}");

                if (response.IsSuccessStatusCode)
                    return new ApiResponse<HttpStatusCode>(response.StatusCode, string.Empty);
                else
                    return new ApiResponse<HttpStatusCode>(HttpStatusCode.ExpectationFailed, await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                return new ApiResponse<HttpStatusCode>(HttpStatusCode.ExpectationFailed, e.GetExceptionMessage());
            }
        }

        public async Task<ApiResponse<HttpStatusCode>> DeleteTeacherCourse(int courseId)
        {
            try
            {
                var response = await client.DeleteAsync($"{root}/classes?id={courseId}");

                if (response.IsSuccessStatusCode)
                    return new ApiResponse<HttpStatusCode>(response.StatusCode, string.Empty);
                else
                    return new ApiResponse<HttpStatusCode>(HttpStatusCode.ExpectationFailed, await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                return new ApiResponse<HttpStatusCode>(HttpStatusCode.ExpectationFailed, e.GetExceptionMessage());
            }
        }

        public async Task<IEnumerable<ISearchable>> GetClasses(string name = "")
        {
            try
            {
                var response = await client.GetAsync($"{root}/classes?perPage=2147483647&pageNumber=1&name={name}");

                if (response.IsSuccessStatusCode)
                    return ResponseFromStream<GetClassesResponse>(await response.Content.ReadAsStreamAsync())?.Data;
                else
                    return new List<ISearchable>();
            }
            catch (Exception e)
            {
                return new List<ISearchable>();
            }
        }

        public async Task<IEnumerable<ISearchable>> GetInstitutions(string name)
        {
            try
            {
                var response = await client.GetAsync($"{root}/institutions?name={name}");

                if (response.IsSuccessStatusCode)
                    return ResponseFromStream<List<Institution>>(await response.Content.ReadAsStreamAsync());
                else
                    return new List<ISearchable>();
            }
            catch (Exception e)
            {
                return new List<ISearchable>();
            }
        }

        public async Task<ApiResponse<HttpStatusCode>> ManageSession(string action, int classId)
        {
            try
            {
                var response = await client.PostAsync($"{root}/session?action={action}&classId={classId}", null);

                if (response.IsSuccessStatusCode)
                    return new ApiResponse<HttpStatusCode>(response.StatusCode, string.Empty);
                else
                    return new ApiResponse<HttpStatusCode>(HttpStatusCode.ExpectationFailed, await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                return new ApiResponse<HttpStatusCode>(HttpStatusCode.ExpectationFailed, e.GetExceptionMessage());
            }
        }

        public async Task<ApiResponse<HttpStatusCode>> GetSession(int classId)
        {
            try
            {
                var response = await client.DeleteAsync($"{root}/session?classId={classId}");

                if (response.IsSuccessStatusCode)
                    return new ApiResponse<HttpStatusCode>(response.StatusCode, string.Empty);
                else
                    return new ApiResponse<HttpStatusCode>(HttpStatusCode.ExpectationFailed, await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                return new ApiResponse<HttpStatusCode>(HttpStatusCode.ExpectationFailed, e.GetExceptionMessage());
            }
        }

        public async Task<ApiResponse<GetAttendanceResponse>> GetAttendance(int perPage, int pageNumber, int classId, string date)
        {
            try
            {
                var response = await client.GetAsync($"{root}/attendance?perPage={perPage}&pageNumber={pageNumber}&classId={classId}&date={date}");

                if (response.IsSuccessStatusCode)
                {
                    var attendance = ResponseFromStream<GetAttendanceResponse>(await response.Content.ReadAsStreamAsync());

                    return new ApiResponse<GetAttendanceResponse>(attendance, string.Empty);
                }
                else
                    return new ApiResponse<GetAttendanceResponse>(null, await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                return new ApiResponse<GetAttendanceResponse>(null, e.GetExceptionMessage());
            }
        }
    }
}