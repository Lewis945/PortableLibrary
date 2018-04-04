using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PortableLibrary.Core.Infrastructure.External.Services
{
    public abstract class BaseExternalProvider
    {
        public virtual string ServiceUri { get; }
        public virtual string ServiceName { get; }
        
        //https://github.com/SixLabors/ImageSharp
        protected async Task<byte[]> GetImageAsByteArray(string imageUri)
        {
            var client = new HttpClient {BaseAddress = new Uri(ServiceUri)};
            var response = await client.GetAsync(imageUri);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                //log this
                //throw new Exception($"The image ({imageUri}) is not found.");
                return null;
            }

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                //log this
                //throw new Exception($"Access to the image ({imageUri}) is forbidden.");
                return null;
            }

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}