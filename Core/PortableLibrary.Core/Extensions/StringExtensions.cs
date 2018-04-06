using System.Text.RegularExpressions;

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

            text = text.Trim();
            text = Regex.Replace(text, @"\s+", " ");
            return text;
        }
    }
}