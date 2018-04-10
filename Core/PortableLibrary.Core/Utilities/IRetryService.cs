using System;
using System.Threading.Tasks;

namespace PortableLibrary.Core.Utilities
{
    public interface IRetryService
    {
        Task<T> ExecuteAsync<T>(Func<Task<T>> func, int maxRetries = 3, TimeSpan? retryInterval = null);
        Task ExecuteAsync(Func<Task> func, int maxRetries = 3, TimeSpan? retryInterval = null);
    }
}
