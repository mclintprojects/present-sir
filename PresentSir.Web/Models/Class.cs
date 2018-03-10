using LiteDB;
using System.ComponentModel.DataAnnotations;

namespace PresentSir.Web.Models
{
    public class Class
    {
        [BsonId]
        public int Id { get; set; }

        [Required]
        public int InstitutionId { get; set; }

        [BsonRef("institution")]
        public Institution Institution { get; set; }

        [Required]
        public string CourseCode { get; set; }

        [Required]
        public int TeacherId { get; set; }
    }

    public class Institution
    {
        [BsonId]
        public int Id { get; set; }

        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}