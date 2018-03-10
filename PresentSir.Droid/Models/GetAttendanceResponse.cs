using System.Collections.Generic;

namespace PresentSir.Droid.Models
{
    internal class GetAttendanceResponse
    {
        public List<Attendance> Data { get; set; }
        public Pagination Pagination { get; set; }
    }
}