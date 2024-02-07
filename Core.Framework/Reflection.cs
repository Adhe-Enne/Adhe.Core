using System.Reflection;
using System;

namespace Core.Framework
{
    public static class Reflection
    {
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        public static void SetProperty(PropertyInfo prop, object obj, string value, string format)
        {
            if (prop.PropertyType == typeof(string))
            {
                value = value?.Trim();
                prop.SetValue(obj, value, null);
            }
            else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                prop.SetValue(obj, Common.ToInteger(value), null);
            else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
            {
                if (string.IsNullOrWhiteSpace(value) || value == "00000000" || value == "00-00-00") return;

                DateTime dtValue;

                if (string.IsNullOrEmpty(format))
                    dtValue = Common.ToDateTime(value);
                else
                    dtValue = DateTime.ParseExact(value, format, System.Globalization.CultureInfo.InvariantCulture);

                //MinValue no es soportado por SQL: 01/01/0001
                if (dtValue == DateTime.MinValue) return;

                prop.SetValue(obj, dtValue, null);
            }
            else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
            {
                prop.SetValue(obj, Common.ToBool(value), null);
            }
            else if (prop.PropertyType == typeof(decimal))
            {
                if (format == null)
                    prop.SetValue(obj, Common.ToDecimal(value), null);
                else
                {

                }
            }
            else if (prop.PropertyType == typeof(decimal?))
            {
                if (format == null)
                    prop.SetValue(obj, Common.ToDecimalNull(value), null);
                else
                {

                }
            }
            else
                throw new Exception("Tipo de dato no soportado");
        }
    }
}
