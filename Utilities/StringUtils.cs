using System.Collections.Generic;
using System.Text;

namespace Etch.OrchardCore.Lever.Utilities
{
    public class StringUtils
    {
        public static string GetOptions(IList<string> items, string selectedValue)
        {
            StringBuilder bld = new StringBuilder();
            bld.Append("<option value=\"\">All</option>\n");
            for (int i = 0; i < items.Count; ++i)
            {
                var value = GetOptionValue(items[i]);

                bld.Append(string.Format("<option value=\"{0}\"{1}>{2}</option>\n",
                                value,
                                !string.IsNullOrEmpty(selectedValue) && selectedValue == value ? " selected=\"selected\"" : "",
                                items[i]));
            }

            return bld.ToString();
        }

        public static string GetOptionValue(string name)
        {
            HashSet<char> removeChars = new HashSet<char>("?&^$#@!()+-,:;<>’\'-_*");
            StringBuilder result = new StringBuilder(name.Length);
            foreach (char c in name.ToLower())
            {
                if (!removeChars.Contains(c))
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }
    }
}
