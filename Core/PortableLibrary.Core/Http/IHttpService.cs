using PortableLibrary.Core.Enums;
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
            NamingCaseType requestNamingCase = NamingCaseType.Pascal,
            NamingCaseType responseNamingCase = NamingCaseType.Pascal,
            string language = null, string authToken = null)
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
            NamingCaseType responseNamingCase = NamingCaseType.Pascal,
            string language = null, string authToken = null)
            where TResponse : class, new();
    }
}