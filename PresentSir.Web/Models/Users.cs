using LiteDB;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace PresentSir.Web.Models
{
    public class ApplicationUser
    {
        [BsonId]
        public int Id { get; set; }

        [Required]
        [ScriptIgnore]
        public string Password { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public AccountType AccountType { get; set; }

        [Required]
        public string FullName { get; set; }

        public string IndexNumber { get; set; }
    }

    public class Teacher
    {
        [BsonId]
        public int Id { get; set; }

        [BsonRef("user")]
        public ApplicationUser User { get; set; }
    }

    public class Student
    {
        [BsonId]
        public int Id { get; set; }

        [BsonRef("user")]
        public ApplicationUser User { get; set; }

        [BsonRef("class")]
        public List<Class> RegisteredClasses { get; set; } = new List<Class>();
    }

    public enum AccountType
    {
        Teacher = 1,
        Student
    }
}