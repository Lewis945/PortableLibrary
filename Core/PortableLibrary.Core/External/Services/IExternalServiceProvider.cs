using System.Threading.Tasks;
using PortableLibrary.Core.External.Models;

namespace PortableLibrary.Core.External.Services
{
    public interface IExternalServiceProvider<T>
        where T : IExternalModel
    {
        string ServiceUri { get; }
        string ServiceName { get; }

        Task<T> Extract();
    }
}