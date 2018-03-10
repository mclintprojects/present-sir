using System;

namespace PresentSir.Droid.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        public int ClassId { get; set; }
        public Student Student { get; set; }

        public DateTime ClassDate { get; set; }
    }
}