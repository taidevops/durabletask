//  ----------------------------------------------------------------------------------
//  Copyright Microsoft Corporation
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  http://www.apache.org/licenses/LICENSE-2.0
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//  ----------------------------------------------------------------------------------

namespace DurableTask.Core
{
    using DurableTask.Core.Exceptions;
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Dispatcher class for fetching and processing work items of the supplied type
    /// </summary>
    /// <typeparam name="T">The typed Object to dispatch</typeparam>
    public class WorkItemDispatcher<T> : IDisposable
    {
        const int DefaultMaxConcurrentWorkItems = 20;
        const int DefaultDispatcherCount = 1;

        const int BackOffIntervalOnInvalidOperationSecs = 10;
        const int CountDownToZeroDelay = 5;

        // ReSharper disable once StaticMemberInGenericType
        static readonly TimeSpan DefaultReceiveTimeout = TimeSpan.FromSeconds(30);
        readonly string id;
        readonly string name;
        readonly object thisLock = new object();
        readonly SemaphoreSlim initializationLock = new SemaphoreSlim(1, 1);

        volatile int concurrentWorkItemCount;
        volatile int countDownToZeroDelay;
        volatile int delayOverrideSecs;
        volatile int activeFetchers;
        bool isStarted;
        SemaphoreSlim concurrencyLock;
        CancellationTokenSource shutdownCancellationTokenSource;

        /// <summary>
        /// Gets or sets the maximum concurrent work items
        /// </summary>
        public int MaxConcurrentWorkItems { get; set; } = DefaultMaxConcurrentWorkItems;

        /// <summary>
        /// Gets or sets the number of dispatchers to create
        /// </summary>
        public int DispatcherCount { get; set; } = DefaultDispatcherCount;

        readonly Func<T, string> workItemIdentifier;

        Func<TimeSpan, CancellationToken, Task<T>> FetchWorkItem { get; }

        Func<T, Task> ProcessWorkItem { get; }

        /// <summary>
        /// Method to execute for safely releasing a work item
        /// </summary>
        public Func<T, Task> SafeReleaseWorkItem;

        /// <summary>
        /// Method to execute for aborting a work item
        /// </summary>
        public Func<T, Task> AbortWorkItem;

        /// <summary>
        /// Method to get a delay to wait after a fetch exception
        /// </summary>
        public Func<Exception, int> GetDelayInSecondsAfterOnFetchException = (exception) => 0;

        /// <summary>
        /// Method to get a delay to wait after a process exception
        /// </summary>
        public Func<Exception, int> GetDelayInSecondsAfterOnProcessException = (exception) => 0;

        /// <summary>
        /// Creates a new Work Item Dispatcher with given name and identifier method
        /// </summary>
        /// <param name="name">Name identifying this dispatcher for logging and diagnostics</param>
        /// <param name="workItemIdentifier"></param>
        /// <param name="fetchWorkItem"></param>
        /// <param name="processWorkItem"></param>
        public WorkItemDispatcher(
            string name,
            Func<T, string> workItemIdentifier,
            Func<TimeSpan, CancellationToken, Task<T>> fetchWorkItem,
            Func<T, Task> processWorkItem)
        {
            this.name = name;
            this.id = Guid.NewGuid().ToString("N");
            this.workItemIdentifier = workItemIdentifier ?? throw new ArgumentNullException(nameof(workItemIdentifier));
            this.FetchWorkItem = fetchWorkItem ?? throw new ArgumentNullException(nameof(fetchWorkItem));
            this.ProcessWorkItem = processWorkItem ?? throw new ArgumentNullException(nameof(processWorkItem));
        }

        /// <summary>
        /// Starts the work item dispatcher
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception if dispatcher has already been started</exception>
        public async Task StartAsync()
        {
            if (!this.isStarted)
            {
                await this.initializationLock.WaitAsync();
                try
                {
                    if (this.isStarted)
                    {
                        
                    }

                    this.concurrencyLock?.Dispose();
                    this.concurrencyLock = new SemaphoreSlim(this.MaxConcurrentWorkItems);

                    this.shutdownCancellationTokenSource?.Dispose();
                    this.shutdownCancellationTokenSource = new CancellationTokenSource();

                    this.isStarted = true;

                    for (var i = 0; i < this.DispatcherCount; i++)
                    {
                        string dispatcherId = i.ToString();
                        var context = new WorkItemDispatcherContext(this.name, this.id, dispatcherId);

                        // We just want this to Run we intentionally don't wait
                        Task.Run(() => this)
                    }

                    await Task.CompletedTask;
                }
                finally
                {
                }
            }
        }

        async Task DispatchAsync(WorkItemDispatcherContext context)
        {
            string dispatcherId = context.DispatcherId;

            bool logThrottle = true;
            while (this.isStarted)
            {
                if (!await this.concurrencyLock.WaitAsync(TimeSpan.FromSeconds(5)))
                {
                    if (logThrottle)
                    {
                        // This can happen frequently under heavy load.
                        // To avoid log spam, we log just once until we can proceed.
                        logThrottle = false;
                    }

                    continue;
                }

                logThrottle = true;

                var delaySecs = 0;
                T workItem = default(T);
                try
                {
                    Interlocked.Increment(ref this.activeFetchers);
                    
                    workItem = await this
                }
                catch (TimeoutException)
                {
                    delaySecs = 0;
                }
                catch (TaskCanceledException exception)
                {
                    
                }
                catch (Exception exception)
                {
                    if (!this.isStarted)
                    {

                    }
                }
                finally
                {
                    Interlocked.Decrement(ref this.activeFetchers);
                }


            }
        }

        static bool IsNull(T value) => Equals(value, default(T));

        async Task ProcessWorkItemAsync(WorkItemDispatcherContext context, object workItemObj)
        {
            var workItem = (T)workItemObj;
            var abortWorkItem = true;
            string workItemId = string.Empty;

            try
            {
                workItemId = this.workItemIdentifier(workItem);

                await this.ProcessWorkItem(workItem);

                abortWorkItem = false;
            }
            catch (TypeMissingException exception)
            {

            }
        }

        void AdjustDelayModifierOnSuccess()
        {
            lock (this.thisLock)
            {
                if (this.countDownToZeroDelay > 0)
                {
                    this.countDownToZeroDelay--;
                }

                if (this.countDownToZeroDelay == 0)
                {
                    this.delayOverrideSecs = 0;
                }
            }
        }

        void AdjustDelayModifierOnFailure(int delaySecs)
        {
            lock (this.thisLock)
            {
                this.delayOverrideSecs = Math.Max(this.delayOverrideSecs, delaySecs);
                this.countDownToZeroDelay = CountDownToZeroDelay;
            }
        }

        async Task ExceptionTraceWrapperAsync(
            WorkItemDispatcherContext context,
            string workItemId,
            string operation,
            Func<Task> asyncAction)
        {
            try
            {
                await asyncAction();
            }
            catch (Exception exception)
            {

            }
        }

        /// <summary>
        /// Method for formatting log messages to inlcude dispatcher name and id information
        /// </summary>
        /// <param name="dispatcherId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public string GetFormattedLog(string dispatcherId, string message)
        {
            return $"{this.name}-{this.id}-{dispatcherId}: {message}";
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                
            }
        }
    }
}
