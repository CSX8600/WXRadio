using System.Text;

namespace WXRadio.WeatherManager.Extensions
{
    public static class StringExtensions
    {
        public static string ToDisplayString(this string value)
        {
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < value.Length; i++)
            {
                if (i == 0)
                {
                    builder.Append(value[i]);
                    continue;
                }

                if (char.IsUpper(value[i]))
                {
                    builder.Append(" ");
                }

                builder.Append(value[i]);
            }

            return builder.ToString();
        }
    }
}
