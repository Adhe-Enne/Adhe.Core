using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Framework
{
    public static class Text
    {
        public static string CollapseSpaces(string value)
        {
            return Regex.Replace(value, @"\s+", " ");
        }

        public static string[] SplitAndTrim(this string text, char separator)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            return text.Split(separator).Select(t => t.Trim()).ToArray();
        }
    }
}
