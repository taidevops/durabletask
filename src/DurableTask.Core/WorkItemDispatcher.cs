////  ----------------------------------------------------------------------------------
////  Copyright Microsoft Corporation
////  Licensed under the Apache License, Version 2.0 (the "License");
////  you may not use this file except in compliance with the License.
////  You may obtain a copy of the License at
////  http://www.apache.org/licenses/LICENSE-2.0
////  Unless required by applicable law or agreed to in writing, software
////  distributed under the License is distributed on an "AS IS" BASIS,
////  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
////  See the License for the specific language governing permissions and
////  limitations under the License.
////  ----------------------------------------------------------------------------------

//namespace DurableTask.Core
//{
//    using System;
//    using System.Diagnostics;
//    using System.Threading;
//    using System.Threading.Tasks;
//    using DurableTask.Core.Common;
//    using DurableTask.Core.Exceptions;
//    using DurableTask.Core.Logging;
//    using DurableTask.Core.Tracing;

//    /// <summary>
//    /// Dispatcher class for fetching and processing work items of the supplied type
//    /// </summary>
//    /// <typeparam name="T">The typed Object to dispatch</typeparam>
//    public class WorkItemDispatcher<T> : IDisposable
//    {
//        const int DefaultMaxConcurrentWorkItems = 20;
//        const int DefaultDispatcherCount = 1;

//        const int BackOffIntervalOnInvalidOperationSecs = 10;
//        const int CountDownToZeroDelay = 5;

//        // ReSharper disable once StaticMemberInGenericType
//        static readonly TimeSpan DefaultReceiveTimeout = TimeSpan.FromSeconds(30);
//        readonly string id;
//        readonly string name;
//        readonly object thisLock = new object();
//        readonly SemaphoreSlim initializationLock = new SemaphoreSlim(1, 1);

//        volatile int concurrentWorkItemCount;
//        volatile int countDownToZeroDelay;
//        volatile int delayOverrideSecs;
//        volatile int activeFetchers;
//        bool isStarted;
//        SemaphoreSlim concurrencyLock;
//        CancellationTokenSource shutdownCancellationTokenSource;

//        /// <summary>
//        /// Gets or sets the maximum concurrent work items
//        /// </summary>
//        public int MaxConcurrentWorkItems { get; set; } = DefaultMaxConcurrentWorkItems;

//        /// <summary>
//        /// Gets or sets the number of dispatchers to create
//        /// </summary>
//        public int DispatcherCount { get; set; } = DefaultDispatcherCount;

//        readonly Func<T, string> workItemIdentifier;

//        Func<TimeSpan, CancellationToken, Task<T>> FetchWorkItem { get; }

//        Func<T, Task> ProcessWorkItem { get; }

//        /// <inheritdoc />
//        public void Dispose()
//        {
//            this.Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        /// <summary>
//        /// Releases unmanaged and - optionally - managed resources.
//        /// </summary>
//        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
//        protected virtual void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                this.initializationLock.Dispose();
//                this.concurrencyLock?.Dispose();
//                this.shutdownCancellationTokenSource?.Dispose();
//            }
//        }
//    }
//}
