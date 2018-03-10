using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PresentSir.Droid.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetExceptionMessage(this Exception e)
        {
            if (e is HttpRequestException)
                return AppResx.NoInternetMessage;
            else if (e is TaskCanceledException)
                return AppResx.SlowInternetMessage;
            else
                return AppResx.SomethingWentWrong;
        }
    }
}