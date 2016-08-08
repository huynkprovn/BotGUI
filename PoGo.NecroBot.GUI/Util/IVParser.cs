using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PoGo.NecroBot.GUI.Util
{
    class IVParser
    {
        public static double ParseIV(string input)
        {
            var iv = ParseRegexDouble(input,
                GeoCoordinatesParser.GeoCoordinatesRegex + @"\W+(?i)IV\W+(1?\d{1,3}(?:[,.]\d{1,3})?)\b");
            // 97,8.200341 IV 98
            if (iv == default(double))
                iv = ParseRegexDouble(input, @"(?i)\b(1?\d{1,3}(?:[,.]\d{1,3})?)\W*\%?\W*IV"); // 52 IV 52% IV 52IV 52.5 IV
            if (iv == default(double))
                iv = ParseRegexDouble(input, @"(?i)\bIV\W?(1?\d{1,2}(?:[,.]\d{1,3})?)");
            if (iv == default(double))
                iv = ParseRegexDouble(input, @"\b(1?\d{1,3}(?:[,.]\d{1,3})?)\W*\%"); // 52% 52 %

            return iv;
        }

        private static double ParseRegexDouble(string input, string regex)
        {
            var match = Regex.Match(input, regex);
            if (match.Success)
            {
                return Convert.ToDouble(match.Groups[1].Value.Replace(',', '.'), CultureInfo.InvariantCulture);
            }
            return default(double);
        }
    }
}
