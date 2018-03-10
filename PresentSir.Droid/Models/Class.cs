namespace PresentSir.Droid.Models
{
    public class Class
    {
        public int Id { get; set; }
        public int InstitutionId { get; set; }
        public Institution Institution { get; set; }
        public string CourseCode { get; set; }
    }

    public class Institution
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}