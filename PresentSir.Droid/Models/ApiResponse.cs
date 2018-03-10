namespace PresentSir.Droid.Models
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string ErrorMessage { get; set; }

        public ApiResponse(T data, string errorMessage)
        {
            Data = data;
            ErrorMessage = errorMessage;
        }
    }
}