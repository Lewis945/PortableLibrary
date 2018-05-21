using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace PortableLibrary.Core.Http
{
    public interface IHttpService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestObject"></param>
        /// <param name="language"></param>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest requestObject,
            string language = null)
            where TRequest : class, new()
            where TResponse : class, new();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <param name="language"></param>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        Task<TResponse> PostStringAsync<TResponse>(string url, string content,
            string language = null)
            where TResponse : class, new();
    }
}