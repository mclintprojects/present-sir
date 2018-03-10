using System;

namespace PresentSir.Web.Extensions
{
    public static class DateExtension
    {
        public static bool IsSameDay(this DateTime date, DateTime otherDate)
        {
            return (date.ToShortDateString() == otherDate.ToShortDateString());
        }
    }
}