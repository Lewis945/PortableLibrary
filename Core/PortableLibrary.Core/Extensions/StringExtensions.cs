namespace PortableLibrary.Core.Extensions
{
    public static class StringExtensions
    {
        public static string FormatAlias(this string name, string postfix = null)
        {
            if (!string.IsNullOrWhiteSpace(postfix))
                return $"{name.ReplaceWrongCharacters()}-{postfix.ReplaceWrongCharacters()}";

            return name.ReplaceWrongCharacters();
        }

        public static string ReplaceWrongCharacters(this string text)
        {
            text = text.ToLowerInvariant();
            text = System.Text.RegularExpressions.Regex.Replace(text, @"[^\w\d\s-]", string.Empty); // Remove all non valid chars          
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ").Trim(); // convert multiple spaces into one space  
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\s", "-"); // //Replace spaces by dashes
            return text;
        }
    }
}