namespace LunarSurfaceOperations.Utilities
{
    using System;

    public static class DateTimeExtensions
    {
        public static long GetUnixTimeMilliseconds(this DateTime date) => (long)date.Subtract(DateTime.UnixEpoch).TotalMilliseconds;
    }
}