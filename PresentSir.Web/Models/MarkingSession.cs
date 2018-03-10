using LiteDB;

namespace PresentSir.Web.Models
{
    public class MarkingSession
    {
        [BsonId]
        public int Id { get; set; }

        public int ClassId { get; set; }
    }
}