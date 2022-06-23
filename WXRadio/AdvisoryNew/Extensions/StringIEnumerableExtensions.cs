using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WXRadio.WeatherManager.Extensions
{
    public static class StringIEnumerableExtensions
    {
        public static string ConcatToString(this IEnumerable<string> enumerable, string concatString)
        {
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < enumerable.Count(); i++)
            {
                string returned = "";
                if (i != 0)
                {
                    returned += concatString + " ";
                }
                returned += enumerable.ElementAt(i);

                builder.Append(returned);
            }

            return builder.ToString();
        }
    }
}
