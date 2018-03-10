using Newtonsoft.Json.Linq;

namespace PresentSir.Droid.Models
{
    internal class LoginResponse
    {
        public string Token { get; set; }
        public ApplicationUser User { get; set; }
        public JObject Details { get; set; }
    }
}