namespace PresentSir.Web.Models
{
    public class USSDRequest
    {
        public int SessionId { get; set; }
        public int ServiceCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Text { get; set; }
    }
}