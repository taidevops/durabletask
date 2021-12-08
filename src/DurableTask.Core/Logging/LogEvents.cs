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

namespace DurableTask.Core.Logging
{
    using System;
    using System.Text;
    using DurableTask.Core.Command;
    using DurableTask.Core.Common;
    using DurableTask.Core.History;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// This class defines all log events supported by DurableTask.Core.
    /// </summary>
    /// <remarks>
    /// Each inner-class represents a single log event that derives from <see cref="StructuredLogEvent"/> and
    /// optionally implements <see cref="IEventSourceEvent"/>.
    /// </remarks>
    static class LogEvents
    {
        internal class TaskHubWorkerStarting : StructuredLogEvent, IEventSourceEvent
        {
            public override EventId EventId => new EventId(
                EventIds.TaskHubWorkerStarted,
                nameof(EventIds.TaskHubWorkerStarted));

            public override LogLevel Level => LogLevel.Information;

            protected override string CreateLogMessage() => "Durable task hub worker is starting";

            void IEventSourceEvent.WriteEventSource() =>
                StructuredEventSource.Log.TaskHubWorkerStarting(Utils.AppName, Utils.PackageVersion);
        }
    }
}
