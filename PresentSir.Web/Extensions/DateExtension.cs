using System;

namespace PresentSir.Web.Extensions
{
    public static class DateExtension
    {
        public static bool IsSameDay(this DateTime date, DateTime otherDate)
        {
            var date1 = $"{date.Day}/{date.Month}/{date.Year}";
            var date2 = $"{otherDate.Day}/{otherDate.Month}/{otherDate.Year}";
            return (date1 == date2);
        }
    }
}