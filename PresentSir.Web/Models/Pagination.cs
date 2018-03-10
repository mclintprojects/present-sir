namespace PresentSir.Web.Models
{
    public class Pagination
    {
        public bool HasNextPage { get; set; }
        public int PageNumber { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}