using System;
namespace Etch.OrchardCore.Lever.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime UnixTimeStampToDateTime(this long unixTimeStamp)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return date.AddMilliseconds(unixTimeStamp);
        }
    }
}
