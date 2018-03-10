using Alansa.Droid.Interfaces;

namespace PresentSir.Droid.Models
{
    public class Class : ISearchable
    {
        public int Id { get; set; }
        public int InstitutionId { get; set; }
        public Institution Institution { get; set; }
        public string CourseCode { get; set; }
        public int TeacherId { get; set; }

        public int GetId() => Id;

        public string GetPrimaryText() => $"{Institution.ShortName} CourseCode";
    }

    public class Institution : ISearchable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        public int GetId() => Id;

        public string GetPrimaryText() => Name;
    }
}