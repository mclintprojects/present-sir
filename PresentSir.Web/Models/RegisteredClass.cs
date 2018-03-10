using LiteDB;

namespace PresentSir.Web.Models
{
    public class RegisteredClass
    {
        [BsonId]
        public int Id { get; set; }

        public int StudentId { get; set; }

        [BsonRef("class")]
        public Class Class { get; set; }
    }
}