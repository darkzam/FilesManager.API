using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FilesManager.API.Helpers
{
    public static class TextHelper
    {
        public static string RemoveAccents(this string text)
        {
            return string.Concat(Regex.Replace(text, @"(?i)[\p{L}-[ña-z]]+",
                                 m => m.Value.Normalize(NormalizationForm.FormD))
                         .Where(c => CharUnicodeInfo.GetUnicodeCategory(c)
                                    != UnicodeCategory.NonSpacingMark));
        }
    }
}
