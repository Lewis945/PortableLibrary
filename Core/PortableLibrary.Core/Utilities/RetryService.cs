using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortableLibrary.Core.Utilities
{
    public class RetryService : IRetryService
    {
        //public static T Execute<T>(Func<T> func, int maxRetries = 3,
        //    int delayInMilliseconds = 250)
        //{
        //    return
        //        ExecuteAsync(async () => await Task.FromResult(func()), maxRetries, delayInMilliseconds)
        //            .Await();
        //}

        //public static void Execute(Action action, int maxRetries = 3,
        //    int delayInMilliseconds = 250)
        //{
        //    ExecuteAsync(async () =>
        //    {
        //        action();
        //        return await Task.FromResult(0);
        //    },  maxRetries, delayInMilliseconds).Await();
        //}

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> func, int maxRetries = 3, TimeSpan? retryInterval = null)
        {
            if (!retryInterval.HasValue)
                retryInterval = TimeSpan.FromSeconds(5);

            var currentRetry = 0;

            var exceptions = new List<Exception>();

            while (true)
            {
                try
                {
                    return await func();
                }
                catch (Exception ex)
                {
                    currentRetry++;

                    exceptions.Add(ex);

                    if (currentRetry > maxRetries)
                    {
                        throw new AggregateException(exceptions);
                    }
                }

                await Task.Delay(retryInterval.Value);
            }
        }

        public async Task ExecuteAsync(Func<Task> func, int maxRetries = 3, TimeSpan? retryInterval = null)
        {
            await ExecuteAsync(async () =>
            {
                await func();
                return await Task.FromResult(0);
            }, maxRetries, retryInterval);
        }
    }
}