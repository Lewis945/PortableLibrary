using System.Threading.Tasks;
using PortableLibrary.Core.External.Models;

namespace PortableLibrary.Core.External.Services
{
    public interface IExternalServiceProvider<T>
        where T : IExternalModel
    {
        string ProviderUri { get; }

        Task<T> Extract();
    }
}