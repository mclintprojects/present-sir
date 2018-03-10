using System.Collections.Generic;

namespace PresentSir.Droid.Models
{
    internal class GetTeacherCoursesResponse
    {
        public List<Class> Data { get; set; }
        public Pagination Pagination { get; set; }
    }
}