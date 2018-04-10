using System;
using System.Collections.Generic;
using System.Threading;

namespace PortableLibrary.Core.Utilities
{
    public static class Retry
    {
        public static void Execute(
            Action action,
            TimeSpan retryInterval,
            int maxAttemptCount = 3)
        {
            Execute<object>(() =>
            {
                action();
                return null;
            }, retryInterval, maxAttemptCount);
        }

        public static T Execute<T>(
            Func<T> action,
            TimeSpan retryInterval,
            int maxAttemptCount = 3)
        {
            var exceptions = new List<Exception>();

            for (int attempted = 0; attempted < maxAttemptCount; attempted++)
            {
                try
                {
                    if (attempted > 0)
                    {
                        Thread.Sleep(retryInterval);
                    }
                    return action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            throw new AggregateException(exceptions);
        }
        
        /// <summary>
        /// Tries to execute a function synchronously. If the execution fails it is retried until max retries is reached.
        /// </summary>
        /// <param name="func">The function to execute</param>
        /// <param name="isTransient">If an exception occures tell based on the exception if a retry should be done (true) or not (false). If the parameter is null a retry will always done (unless max retries is reached).</param>
        /// <param name="maxRetries">The maximum number of retries. For one it will be executed once and then retried only a single time.</param>
        /// <param name="delayInMilliseconds">The time to wait before the next execution</param>
        /// <param name="logger">Instance of logger for retries.</param>
        public static T Execute<T>(Func<T> func, Func<Exception, bool> isTransient = null, int maxRetries = 3,
            int delayInMilliseconds = 250, IRetriedFunctionLog logger = null)
        {
            return
                ExecuteAsync(async () => await Task.FromResult(func()), isTransient, maxRetries, delayInMilliseconds, logger)
                    .Await();
        }

        /// <summary>
        /// Tries to execute an action synchronously. If the execution fails it is retried until max retries is reached.
        /// </summary>
        /// <param name="action">The action to execute</param>
        /// <param name="isTransient">If an exception occures tell based on the exception if a retry should be done (true) or not (false). If the parameter is null a retry will always done (unless max retries is reached).</param>
        /// <param name="maxRetries">The maximum number of retries. For one it will be executed once and then retried only a single time.</param>
        /// <param name="delayInMilliseconds">The time to wait before the next execution</param>
        /// <param name="logger">Instance of logger for retries.</param>
        public static void Execute(Action action, Func<Exception, bool> isTransient = null, int maxRetries = 3,
            int delayInMilliseconds = 250, IRetriedFunctionLog logger = null)
        {
            ExecuteAsync(async () =>
            {
                action();
                return await Task.FromResult(0);
            }, isTransient, maxRetries, delayInMilliseconds, logger).Await();
        }

        /// <summary>
        /// Tries to execute a function asynchronously. If the execution fails it is retried until max retries is reached.
        /// </summary>
        /// <param name="func">The awaitable function to execute</param>
        /// <param name="isTransient">If an exception occures tell based on the exception if a retry should be done (true) or not (false). If the parameter is null a retry will always done (unless max retries is reached).</param>
        /// <param name="maxRetries">The maximum number of retries. For one it will be executed once and then retried only a single time.</param>
        /// <param name="delayInMilliseconds">The time to wait before the next execution</param>
        /// <param name="logger">Instance of logger for retries.</param>
        public static async Task<T> ExecuteAsync<T>(Func<Task<T>> func, Func<Exception, bool> isTransient = null,
            int maxRetries = 3,
            int delayInMilliseconds = 250, IRetriedFunctionLog logger = null)
        {
            var currentRetry = 0;

            while (true)
            {
                try
                {
                    return await func();
                }
                catch (CancelJobException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    currentRetry++;

                    if (ex.InnerException != null && ex.InnerException is CancelJobException)
                        throw ex.InnerException;

                    if (logger != null)
                    {
                        // Get transient indicator.
                        var isFuncTransient = false;
                        if (isTransient != null) isFuncTransient = isTransient(ex);

                        // Log retry attempt.
                        logger.Log(ex, isFuncTransient, currentRetry);
                    }

                    if (currentRetry > maxRetries || (isTransient != null && !isTransient(ex)))
                    {
                        throw;
                    }
                }

                await Task.Delay(delayInMilliseconds);
            }
        }

        /// <summary>
        /// Tries to execute a function asynchronously. If the execution fails it is retried until max retries is reached.
        /// </summary>
        /// <param name="func">The awaitable function to execute</param>
        /// <param name="isTransient">If an exception occures tell based on the exception if a retry should be done (true) or not (false). If the parameter is null a retry will always done (unless max retries is reached).</param>
        /// <param name="maxRetries">The maximum number of retries. For one it will be executed once and then retried only a single time.</param>
        /// <param name="delayInMilliseconds">The time to wait before the next execution</param>
        /// <param name="logger">Instance of logger for retries.</param>
        public static async Task ExecuteAsync(Func<Task> func, Func<Exception, bool> isTransient = null,
            int maxRetries = 3, int delayInMilliseconds = 250, IRetriedFunctionLog logger = null)
        {
            await ExecuteAsync(async () =>
            {
                await func();
                return await Task.FromResult(0);
            }, isTransient, maxRetries, delayInMilliseconds, logger);
        }
    }
}