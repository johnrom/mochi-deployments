
using System;

namespace Nmbl.Deployments.Vercel.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDateTimeUtc(this long dateTime)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(dateTime).UtcDateTime;
        }
    }
}
