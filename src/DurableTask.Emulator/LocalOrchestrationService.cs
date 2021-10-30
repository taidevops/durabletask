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
    using DurableTask.Core.Common;
    using DurableTask.Core.Exceptions;
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
    public class LocalOrchestrationService : IOrchestrationService
    {
        //readonly List<TaskMessage> timerMessages;

        //readonly CancellationTokenSource cancellationTokenSource;

        readonly object timerLock = new object();

        //async Task TimerMessageSchedulerAsync()
        //{
        //    while (!this.cancellationTokenSource.Token.IsCancellationRequested)
        //    {
        //        lock (this.timerLock)
        //        {
        //            foreach (TaskMessage tm in this.timerMessages.ToList())
        //            {
        //                var te = tm.Event as TimerFiredEvent;

        //                if (te == null)
        //                {
        //                    // TODO : unobserved task exception (AFFANDAR)
        //                    throw new InvalidOperationException("Invalid timer message");
        //                }

        //                if (te.FireAt <= DateTime.UtcNow)
        //                {
        //                    this.orch
        //                }
        //            }    
        //        }
        //    }
        //}

        /******************************/
        // management methods
        /******************************/
        /// <inheritdoc />
        public Task CreateAsync()
        {
            return CreateAsync(true);
        }

        /// <inheritdoc />
        public Task CreateAsync(bool recreateInstanceStore)
        {
            return Task.FromResult<object>(null);
        }

        /// <inheritdoc />
        public Task StartAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task StopAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task StopAsync(bool isForced)
        {
            throw new NotImplementedException();
        }
    }
}
