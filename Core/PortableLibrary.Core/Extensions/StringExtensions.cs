namespace PortableLibrary.Core.Extensions
{
    public static class StringExtensions
    {
        public static string FormatAlias(this string name)
        {
            string title = name.ToLowerInvariant();
            title = System.Text.RegularExpressions.Regex.Replace(title, @"[^\w\d\s-]", string.Empty); // Remove all non valid chars          
            title = System.Text.RegularExpressions.Regex.Replace(title, @"\s+", " ").Trim(); // convert multiple spaces into one space  
            title = System.Text.RegularExpressions.Regex.Replace(title, @"\s", "-"); // //Replace spaces by dashes
            return title;
        }
    }
}