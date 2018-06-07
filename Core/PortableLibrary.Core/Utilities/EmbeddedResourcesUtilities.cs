using System;
using System.IO;

namespace PortableLibrary.Core.Utilities
{
    public static class EmbeddedResourcesUtilities
    {
        public static string GetEmbeddedResourceContent<T>(string name)
        {
            var assembly = typeof(T).Assembly;

            using (var stream = assembly.GetManifestResourceStream(name))
            {
                if (stream == null)
                    throw new ArgumentException($"Embedded resource {name} is not found", nameof(name));

                using (var reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    return result;
                }
            }
        }
    }
}