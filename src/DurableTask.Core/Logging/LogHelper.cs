﻿//  ----------------------------------------------------------------------------------
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

namespace DurableTask.Core.Logging
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;

    class LogHelper
    {
        readonly ILogger log;

        public LogHelper(ILogger log)
        {
            // nut is ikey
            this.log = log;
        }

        bool IsStructuredLoggingEnabled => this.log != null;

        #region TaskHubWorker
        /// <summary>
        /// Logs that a <see cref="TaskHubWorker"/> is starting.
        /// </summary>
        internal void TaskHubWorkerStarting()
        {
            if (this.IsStructuredLoggingEnabled)
            {
                this.WriteStructuredLog(new LogEvents.TaskHubWorkerStarting());
            }
        }
        #endregion

        void WriteStructuredLog(ILogEvent logEvent, Exception exception = null)
        {
            this.log?.LogDurableEvent(logEvent, exception);
        }
    }
}