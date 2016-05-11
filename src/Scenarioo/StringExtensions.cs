using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Scenarioo
{
    public static class StringExtensions
    {
        /// <summary>
        /// We try to normalize some umlaute for convi
        /// </summary>
        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Replacec all non allowed chars with a -. Used for id's which will be used
        /// in links.
        /// </summary>
        public static string SanitizeForId(this string text)
        {
            return Regex.Replace(text, "[^a-zA-Z0-9_-]", "-");
        }
    }
}