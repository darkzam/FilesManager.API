using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

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

        public static string GetRemoteId(this string url)
        {
            try
            {
                var uri = new Uri(url, UriKind.Absolute);

                string remoteId = Uri.UnescapeDataString(HttpUtility.ParseQueryString(uri.Query).Get("id"));

                return remoteId;
            }
            catch
            {
                return null;
            }
        }
    }
}
