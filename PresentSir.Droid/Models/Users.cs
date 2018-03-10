using Newtonsoft.Json;
using System.Collections.Generic;

namespace PresentSir.Droid.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string Password { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

        public string Username { get; set; }

        public AccountType AccountType { get; set; }

        public string FullName { get; set; }

        public string IndexNumber { get; set; }
    }

    public class Teacher
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
    }

    public class Student
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public List<Class> RegisteredClasses { get; set; } = new List<Class>();
    }

    public enum AccountType
    {
        Teacher = 1,
        Student
    }
}