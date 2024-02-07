using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Core.Framework.Utils
{
    public static class Week
    {
        public static DateTime? FirstDateOfWeek(int year, int weekOfYear)
        {
            if (weekOfYear < 1 || weekOfYear > 52) return null;

            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }

        public static int GetWeekNumber(DateTime date)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }

        public static int CurrentWeekNumber()
        {
            return GetWeekNumber(DateTime.Today);
        }

        public static int NextWeekNumber()
        {
            return CurrentWeekNumber() + 1;
        }
    }
}
