using System.Threading.Tasks;
using PortableLibrary.Core.External.Models;

namespace PortableLibrary.Core.External.Services
{
    public interface IExternalServiceProvider<T>
        where T : IExternalModel
    {
        Task<T> Extract(string uri);
    }
}