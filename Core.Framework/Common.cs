using System;
using System.Globalization;

namespace Core.Framework
{
    public static class Common
    {
        public static bool IsInteger(object data)
        {
            return (ToInteger(data) != int.MinValue);
        }
        public static int ToInteger(object data)
        {
            int aux = int.MinValue;

            if (data != null && data.GetType() == typeof(byte[]))
            {
                string str = string.Empty;
                byte[] l = (byte[])data;
                foreach (byte b in l)
                    str += (char)b;

                if (int.TryParse(str, out aux))
                    return aux;
                else
                    return int.MinValue;
            }
            else if (data != null && data.ToString() != string.Empty)
            {
                if (int.TryParse(data.ToString().Trim(), out aux))
                    return aux;
                else
                    return int.MinValue;
            }

            return aux;
        }
        public static int ToInteger(object data, int def)
        {
            int aux = ToInteger(data);
            if (aux == int.MinValue)
                return def;
            else
                return aux;
        }

        public static bool IsInteger16(object data)
        {
            return (ToInteger16(data) != Int16.MinValue);
        }
        public static Int16 ToInteger16(object data)
        {
            Int16 aux = Int16.MinValue;

            if (data != null && data.GetType() == typeof(byte[]))
            {
                string str = string.Empty;
                byte[] l = (byte[])data;
                foreach (byte b in l)
                    str += (char)b;

                if (Int16.TryParse(str, out aux))
                    return aux;
                else
                    return Int16.MinValue;
            }
            else if (data != null && data.ToString() != string.Empty)
            {
                if (Int16.TryParse(data.ToString().Trim(), out aux))
                    return aux;
                else
                    return Int16.MinValue;
            }

            return aux;
        }
        public static Int16 ToInteger16(object data, Int16 def)
        {
            Int16 aux = ToInteger16(data);
            if (aux == Int16.MinValue)
                return def;
            else
                return aux;
        }

        public static bool IsDateTime(object data)
        {
            bool result = false;

            if (ToDateTime(data) != DateTime.MinValue)
                result = true;

            return result;
        }
        public static DateTime ToDateTime(object data)
        {
            DateTime aux = DateTime.MinValue;
            if (data != null && data.ToString() != string.Empty)
            {
                if (DateTime.TryParse(data.ToString(), new System.Globalization.CultureInfo("es-AR"), System.Globalization.DateTimeStyles.None, out aux))
                    //if (DateTime.TryParse(data.ToString(), out aux))
                    return aux;
                else
                    return DateTime.MinValue;
            }
            return aux;
        }
        public static DateTime ToDateTime(object data, DateTime def)
        {
            DateTime aux = ToDateTime(data);
            if (aux == DateTime.MinValue)
                return def;
            else
                return aux;
        }
        public static DateTime? ToDateTimeNull(object data)
        {
            DateTime dt = ToDateTime(data);

            if (dt == DateTime.MinValue)
                return null;

            return dt;
        }

        public static bool IsLong(object data)
        {
            bool result = false;

            if (ToLong(data) != long.MinValue)
                result = true;

            return result;
        }
        public static long ToLong(object data)
        {
            long aux = long.MinValue;
            if (data != null && data.ToString() != string.Empty)
            {
                if (long.TryParse(data.ToString().Trim(), out aux))
                    return aux;
                else
                    return long.MinValue;
            }
            return aux;
        }
        public static long ToLong(object data, long def)
        {
            long aux = ToLong(data);
            if (aux == long.MinValue)
                return def;
            else
                return aux;
        }

        public static bool IsDouble(object data)
        {
            bool result = false;

            if (ToDouble(data) != double.MinValue)
                result = true;

            return result;
        }
        public static double ToDouble(object data)
        {
            double aux = double.MinValue;

            if (data != null && data.ToString() != string.Empty)
            {
                if (double.TryParse(data.ToString(), out aux))
                    return aux;
                else
                    return double.MinValue;
            }
            return aux;
        }
        public static double ToDouble(object data, double def)
        {
            double aux = ToDouble(data);
            if (aux == double.MinValue)
                return def;
            else
                return aux;
        }

        public static bool IsDecimal(object data)
        {
            bool result = false;

            if (ToDecimal(data) != decimal.MinValue)
                result = true;

            return result;
        }
        public static decimal ToDecimal(object data)
        {
            decimal aux = decimal.MinValue;

            if (data != null && data.ToString() != string.Empty)
            {
                string data2 = data.ToString();

                //Si tiene ambos simbolos "." y "," el primero que aparezca será el separador de miles. 
                //lo elimina de todo el string para evitar problemas
                // si data fuese 1,000.000 (1000 sería el valor correcto y de hacer esto se almacenaría 1000000)
                // si data fuese 1,000,000.000 (1000000 sería el valor correcto y de no hacer esto se almacenaría 1000000000)
                int posPunto = data2.IndexOf(".");
                int posComa = data2.IndexOf(",");

                if (posPunto > -1 && posComa > -1)
                {
                    if (posPunto < posComa)
                        data2 = data2.Replace(".", "");
                    else
                        data2 = data2.Replace(",", "");
                }

                data2 = data2.Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                data2 = data2.Replace(",", System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);

                if (decimal.TryParse(data2, out aux))
                    return aux;
                else
                    return decimal.MinValue;
            }
            return aux;
        }
        public static decimal ToDecimal(object data, decimal def)
        {
            decimal aux = ToDecimal(data);

            if (aux == decimal.MinValue)
                return def;
            else
                return aux;
        }
        public static decimal? ToDecimalNull(object data)
        {
            decimal d = ToDecimal(data);

            if (d == decimal.MinValue)
                return null;

            return d;
        }

        /// <summary>
        /// Convierte un dato a string y elimina espacios
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToString(object data)
        {
            if (data == null)
                return string.Empty;
            else if (data is DateTime)
                return ((DateTime)data).ToString("dd/MM/yyyy");
            else if (data is decimal)
                return string.Format("{0:0.00}", data);

            return data.ToString().Trim();
        }
        public static string ToString(object data, int length)
        {
            string str = ToString(data);

            if (str.Length > length)
                return str.Substring(0, length);

            return str;
        }

        /// <summary>
        /// Devuelve un String a partir de un String de Fecha y un Formato.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToStringFormat(string date, string format)
        {
            var day = DateTime.Parse(date);

            return ToString(day, format);
        }
        /// <summary>
        /// Recibe un string y devuelve su fragmento a partir del index y la longitud indicada.
        /// Devuelve un string vacio si recibe un objeto Null
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// 
        public static string ToString(object data, int startIndex, int length)
        {
            string str = ToString(data);

            if (str.Length >= startIndex + length)
                return str.Substring(startIndex, length);


            return str;
        }

        public static string ToString(object data, string format)
        {
            if (string.IsNullOrEmpty(format) || data is string)
                return ToString(data);
            else if (data is decimal)
                return ((decimal)data).ToString(format);
            else if (data is decimal?)
            {
                if (data == null) return format;

                return ((decimal?)data).Value.ToString(format);
            }
            else if (data is DateTime)
                return ((DateTime)data).ToString(format);
            else if (data is DateTime?)
            {
                if (data != null)
                    return ((DateTime?)data).Value.ToString(format);
            }

            return string.Empty;
        }
        /// <summary>
        /// Recibe un string de una fecha, un formato de esa fecha y un formato esperado para convertirla.
        /// Devuelve un string con el formato de fecha outputDateFormat, si no es una fecha devuelve null
        /// </summary>
        public static string ToStringFormat(string data, string inputDateFormat, string outputDateFormat)
        {
            DateTime aux;

            if (DateTime.TryParseExact(data, inputDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out aux))
                return aux.ToString(outputDateFormat);
            
            return null;
        }
        
        public static string ExtractNumbers(string data)
        {
            return System.Text.RegularExpressions.Regex.Replace(data, "[^0-9]", "");
        }

        public static bool ToBool(object data)
        {
            if (data == null) return false;

            string val = ToString(data);

            val = val.ToLower();

            if (val == "y" || val == "yes" || val == "s" || val == "si" || val == "true" || val == "1")
                return true;

            return false;
        }

        public static string BoolToBit(bool data)
        {
            if (data)
                return "1";

            return "0";
        }

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

        public static class Others
        {
            public static long CUIL(string str)
            {
                str = str.Replace("-", "");
                str = str.Trim();

                while (str.IndexOf(" ") > -1)
                    str = str.Replace(" ", "");

                return Common.ToLong(str);
            }
        }

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
                if (str.Length != 6 || !IsInteger(str)) return DateTime.MinValue;

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

            public static int ToNumeric(DateTime date)
            {
                return int.Parse(date.ToString("yyyyMMdd"));
            }
        }
    }
}
