using System;
namespace SynaptikonFramework.Util{
    public static class TimestampUtil
    {
        private static readonly DateTime zero = new DateTime(1970, 1, 1);

        public static long NowMilliseconds(){
            long totalMilliseconds = (long)(DateTime.UtcNow.Subtract(zero)).TotalMilliseconds;
            return totalMilliseconds;
		}

        public static long NowUnixTimestamp(){
            long unixTimestamp = (long)(DateTime.UtcNow.Subtract(zero)).TotalSeconds;
            return unixTimestamp;
        }

		public static long NowUnixTimestamp(DateTime date)
		{
			long unixTimestamp = (long)(date.Subtract(zero)).TotalSeconds;
			return unixTimestamp;
        }

        public static DateTime UnixTimestampToDate(this string unixTimestamp)
        {
            var seconds = long.Parse(unixTimestamp);
            DateTime date = zero.AddSeconds(seconds);
            return date;
        }

        public static DateTime UnixTimestampToDate(this long unixTimestamp)
        {
            DateTime date = zero.AddSeconds(unixTimestamp);
            return date;
        }

        public static TimeSpan SinceDateZero(this DateTime dateTime)
        {
            return dateTime.Subtract(zero);
        }
    }
}