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
    using System.Diagnostics.Tracing;
    using System.Threading;

    // NOTE: This is intended to eventually replace the other DurableTask-Core provider
    /// <summary>
    /// Event source logger for DurableTask.Core that uses structured events rather than log messages.
    /// </summary>
    [EventSource(Name = "DurableTask-Core")]
    class StructuredEventSource : EventSource
    {
        internal static readonly StructuredEventSource Log = new StructuredEventSource();

        static readonly AsyncLocal<Guid> ActivityIdState = new AsyncLocal<Guid>();

        [NonEvent]
        public static void SetLogicalTraceActivityId(Guid activityId)
        {
            // We use AsyncLocal to preserve activity IDs across async/await boundaries.
            ActivityIdState.Value = activityId;
            SetCurrentThreadActivityId(activityId);
        }

        [NonEvent]
        internal static void EnsureLogicalTraceActivityId()
        {
            Guid currentActivityId = ActivityIdState.Value;
            if (currentActivityId != CurrentThreadActivityId)
            {
                SetCurrentThreadActivityId(currentActivityId);
            }
        }

        bool IsEnabled(EventLevel level) => this.IsEnabled(level, EventKeywords.None);

        [Event(EventIds.TaskHubWorkerStarting, Level = EventLevel.Informational, Version = 1)]
        internal void TaskHubWorkerStarting(string AppName, string ExtensionVersion)
        {
            if (this.IsEnabled(EventLevel.Informational))
            {
                this.WriteEvent(EventIds.TaskHubWorkerStarting, AppName, ExtensionVersion);
            }
        }
    }
}
