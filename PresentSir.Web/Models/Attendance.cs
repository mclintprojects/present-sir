using LiteDB;
using System;

namespace PresentSir.Web.Models
{
    public class Attendance
    {
        [BsonId]
        public int Id { get; set; }

        public int ClassId { get; set; }

        [BsonRef("student")]
        public Student Student { get; set; }

        public DateTime ClassDate { get; set; }
    }
}