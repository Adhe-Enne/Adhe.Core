using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Core.Framework
{
    public static class Xml
    {
        /// <summary>
        /// Allows empty
        /// </summary>
        /// <param name="node"></param>
        /// <param name="name"></param>
        /// <param name="allowNoTag"></param>
        /// <returns></returns>
        public static string SelectSingleNodeText(this XmlNode node, string name, bool allowNoTag = true)
        {
            return node.SelectSingleNodeText(name, true, allowNoTag);
        }

        public static string SelectSingleNodeText(this XmlNode node, string name, bool allowEmpty, bool allowNoTag)
        {
            var aux = node.SelectSingleNode(name);

            if (aux == null)
            {
                if (allowNoTag)
                    return null;
                else
                    throw new FormatException("No se encontro el tag " + name);
            }
            else if (!allowEmpty && string.IsNullOrWhiteSpace(aux.InnerText.Trim()))
                throw new FormatException($"El tag {name} no puede ser vacío");


            return node.SelectSingleNode(name).InnerText.Trim();
        }

        public static string XMLTag(string tag, string value)
        {
            return "<" + tag + ">" + value + "</" + tag + ">";
        }

        public static void RemoveNamespace(XmlDocument doc)
        {
            const string XMLNS = "xmlns=\"";
            int pos = doc.InnerXml.IndexOf(XMLNS);

            //Quita los namespace para que funciona SelectSingleNode
            while (pos > -1)
            {
                string toReplace = doc.InnerXml.Substring(pos + 7);
                pos = toReplace.IndexOf("\"");
                toReplace = toReplace.Substring(0, pos + 1);
                toReplace = XMLNS + toReplace;

                doc.InnerXml = doc.InnerXml.Replace(toReplace, "");

                pos = doc.InnerXml.IndexOf(XMLNS);
            }
            
        }

        public static string Linearize(string dirtyXml)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@">\s*<");
            string cleanedXml = regex.Replace(dirtyXml, "><");

            cleanedXml = cleanedXml.Replace(Environment.NewLine, "");

            return cleanedXml;
        }

        public static string EscapeSpecialChatacters(string str)
        {
            if (str == null) return string.Empty;

            StringBuilder sb = new StringBuilder();

            foreach (char c in str)
            {
                //Evalua los caracteres y reemplaza o quita
                switch (c)
                {
                    case '&':
                        sb.Append("&amp;");
                        break;
                    case '<':
                        sb.Append("&lt;");
                        break;
                    case '>':
                        sb.Append("&gt;");
                        break;
                    case '\'':
                    case '\\':
                        break;
                    case '"':
                        sb.Append("&quot;");
                        break;
                    case 'º':
                        sb.Append("°");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }

            return sb.ToString();
        }
    }
}
