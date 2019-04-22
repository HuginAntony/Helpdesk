using System;

namespace Helpdesk.Core.Helpers
{
    public static class DateHelper
    {
        public static string MilliSeconds()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HHmmssffff");
        }

        public static string HourMinuteSecond()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToDate(this string dateText)
        {
            DateTime thisDate;

            if (string.IsNullOrEmpty(dateText)) return "";

            if (DateTime.TryParse(dateText, out thisDate))
                return thisDate.ToString("yyyy/MM/dd");

            return dateText;
        }
    }
}