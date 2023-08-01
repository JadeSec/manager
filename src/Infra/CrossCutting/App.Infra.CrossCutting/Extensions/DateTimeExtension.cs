using System;
using System.Text.RegularExpressions;

namespace App.Infra.CrossCutting.Extensions
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// ex:  2019-10-10 00:00:00
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static DateTime TimeCustom(this DateTime dateTime, TimeSpan timeSpan)
            => DateTime.Parse($"{dateTime.ToString("yyyy-MM-dd")} {timeSpan}");

        /// <summary>
        /// 2019-12-31
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime LastDay(this DateTime dateTime)
            => new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));

        /// <summary>
        /// 2019-12-01
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime FirstDay(this DateTime dateTime)
            => new DateTime(dateTime.Year, dateTime.Month, 1);

        /// <summary>
        /// Util para quando se quer pegar um final de semana.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static DateTime GoUntilDayOfWeek(this DateTime dateTime, DayOfWeek dayOfWeek)
        {
            while (dateTime.DayOfWeek != dayOfWeek)
                dateTime = dateTime.AddDays(1);

            return dateTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="timezone">ex: -01,+01,+1,-1, +01:00, -01:00, 01:22, -01:22</param>
        /// <returns></returns>
        public static DateTime WithTimezone(this DateTime dateTime, string timezone)
        {
            try
            {
                //+01:00, -01:00, 01:22, -01:22
                var fullPattern = new Regex("([-+])?([0-9]{1,2}):?([0-9]{2})?");

                if (fullPattern.IsMatch(timezone))
                {
                    var match = fullPattern.Match(timezone);

                    int hor = 0;
                    int min = 0;

                    if (match.Groups[2].Value != null)
                        hor = int.Parse(match.Groups[2].Value);

                    if (match.Groups[3].Value != null)
                        min = int.Parse(match.Groups[3].Value);

                    //+, -
                    string type = match.Groups[1].Value;                                     

                    if(string.IsNullOrEmpty(type) || type == "+")
                    {
                        return dateTime.AddHours(hor)
                                       .AddMinutes(min);
                    }
                    else
                    {
                        return dateTime.AddHours(-hor)
                                       .AddMinutes(-min);

                    }                   
                }
            }
            catch
            {

            }

            return dateTime;
        }
    }
}
