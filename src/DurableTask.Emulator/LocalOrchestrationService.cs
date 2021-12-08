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

namespace DurableTask.Emulator
{
    using DurableTask.Core;
    using DurableTask.Core.History;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Fully functional in-proc orchestration service for testing
    /// </summary>
    public class LocalOrchestrationService : IOrchestrationService, IOrchestrationServiceClient, IDisposable
    {
        readonly CancellationTokenSource cancellationTokenSource;

        readonly object timerLock = new object();

        /// <summary>
        ///     Creates a new instance of the LocalOrchestrationService with default settings
        /// </summary>
        public LocalOrchestrationService()
        {
            this.cancellationTokenSource = new CancellationTokenSource();
        }

        async Task TimerMessageSchedulerAsync()
        {
            while (!this.cancellationTokenSource.Token.IsCancellationRequested)
            {
                lock (this.timerLock)
                {

                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        /// <inheritdoc />
        public Task StartAsync()
        {
            Task.Run(() => TimerMessageSchedulerAsync());
            return Task.FromResult<object>(null);
        }

        /// <inheritdoc />
        public Task StopAsync()
        {
            this.cancellationTokenSource.Cancel();
            return Task.FromResult<object>(null);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.cancellationTokenSource.Cancel();
                this.cancellationTokenSource.Dispose();
            }
        }
    }
}