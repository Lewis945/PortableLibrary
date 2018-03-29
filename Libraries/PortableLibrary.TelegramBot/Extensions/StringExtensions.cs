namespace PortableLibrary.TelegramBot.Extensions
{
    public static class StringExtensions
    {
        public static bool Matches<T>(this string line, T enumItem)
            where T : struct =>
            line.ToLowerInvariant().Equals(enumItem.ToString().ToLowerInvariant());
    }
}