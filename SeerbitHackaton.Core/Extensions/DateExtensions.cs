global using System.Globalization;

namespace SeerbitHackaton.Core.Extensions
{
    public static class DateExtensions
    {
        public static DateTime GetDateUtcNow(this DateTime now)
        {
            return DateTime.UtcNow;
        }

        public static DateTime ToInvariantDateTime(this String value, String format, out bool succeeded)
        {
            DateTimeFormatInfo dtfi = DateTimeFormatInfo.InvariantInfo;
            var result = DateTime.TryParseExact(value, format, dtfi, DateTimeStyles.None, out DateTime newValue);
            succeeded = result;
            return newValue;
        }

        public static string ToDateString(this DateTime dt, String format)
        {
            return dt.ToString(format, DateTimeFormatInfo.InvariantInfo);
        }
    }
}