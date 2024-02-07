using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Framework.Utils
{
    public static class _DateTime
    {
        /// <summary>
        /// Resta 2 fecha y devuelve en minutos
        /// </summary>
        /// <returns></returns>
        public static double Resta(DateTime dt1, DateTime dt2)
        {
            DateTime oldDate = dt1;
            DateTime newDate = dt2;

            // Difference in days, hours, and minutes.
            TimeSpan ts = newDate - oldDate;

            return ts.TotalMinutes;
        }

        public static DateTime FromJulianFormat(object data)
        {
            string str = data.ToString();
            if (str.Length != 6 || !Common.IsInteger(str)) return DateTime.MinValue;

            string y = (19 + int.Parse(str.Substring(0, 1))).ToString() + str.Substring(1, 2);
            int d = int.Parse(str.Substring(3, 3));
            return new DateTime(int.Parse(y), 1, 1).AddDays(d - 1);
        }
        public static string ToJulianFormat(DateTime date)
        {
            return "1" + date.ToString("yy") + date.DayOfYear.ToString("000");
        }

        public static DateTime JoinTime(DateTime date, string time)
        {
            return Common.ToDateTime(Common.ToString(date) + " " + time);
        }

        public static DateTime JoinTime(string date, string time)
        {
            return Common.ToDateTime(date + " " + time);
        }

        public static string MinutesToTime(int minutes)
        {
            string result = "";

            string horas = (minutes / 60).ToString("00");
            string minutos = (minutes - (Common.ToInteger(horas) * 60)).ToString("00");

            if (minutos.StartsWith("-"))
            {
                minutos = minutos.Substring(1, 2);

                if (horas != "00")
                    result = "-";
            }

            result += string.Format("{0}:{1}", horas, minutos);

            return result;
        }
    }
}
