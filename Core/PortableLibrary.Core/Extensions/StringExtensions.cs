using System.Text.RegularExpressions;
using System.Web;

namespace PortableLibrary.Core.Extensions
{
    public static class StringExtensions
    {
        public static string FormatAlias(this string name, string postfix = null)
        {
            return !string.IsNullOrWhiteSpace(postfix)
                ? $"{name.ReplaceWrongCharacters()}-{postfix.ReplaceWrongCharacters()}"
                : name.ReplaceWrongCharacters();
        }

        public static string ReplaceWrongCharacters(this string text)
        {
            text = text.ToLowerInvariant();
            text = Regex.Replace(text, @"[^\w\d\s-]",
                string.Empty);
            text = Regex.Replace(text, @"\s+", " ")
                .Trim();
            text = Regex.Replace(text, @"\s", "-");
            return text;
        }

        public static string AppendUriPath(this string uri, string path)
        {
            string processedUri = uri;
            if (processedUri.EndsWith("/"))
                processedUri = processedUri.Substring(0, processedUri.Length - 1);

            string processedPath = path;
            if (processedPath.StartsWith("/"))
                processedPath = processedPath.Substring(1);
            if (processedPath.EndsWith("/"))
                processedPath = processedPath.Substring(0, processedPath.Length - 1);
            if (!Regex.IsMatch(processedPath, @".+\..+"))
                processedPath = $"{processedPath}/";

            return $"{processedUri}/{processedPath}";
        }

        public static string ClearString(this string text)
        {
            if (text == null)
                return null;

            text = HttpUtility.HtmlDecode(text);
            text = text.Trim();
            text = Regex.Replace(text, @"\s+", " ");
            return text;
        }

        public static string RemoveNewLines(this string text)
        {
            if (text == null)
                return null;

            text = Regex.Replace(text, @"[\r\n]+", string.Empty);
            return text;
        }

        public static string ExtractNumberSubstring(this string text)
        {
            return text == null ? null : Regex.Match(text, @"\d+").Value;
        }

        public static int? ParseNumber(this string text)
        {
            var numberSubstring = text.ExtractNumberSubstring();

            return int.TryParse(numberSubstring, out var number) ? number : default(int?);
        }
    }
}