using System.Collections.Generic;

namespace PresentSir.Droid.Models
{
    internal class GetClassesResponse
    {
        public List<Class> Data { get; set; }
        public Pagination Pagination { get; set; }
    }
}