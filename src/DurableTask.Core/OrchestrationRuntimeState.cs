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
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using DurableTask.Core.Common;
    using DurableTask.Core.History;
    using DurableTask.Core.Tracing;
    using DurableTask.Core.Tracking;

    /// <summary>
    /// Represents the runtime state of an orchestration
    /// </summary>
    public class OrchestrationRuntimeState
    {
        /// <summary>
        /// List of all history events for this runtime state
        /// </summary>
        public IList<HistoryEvent> Events { get; }

        /// <summary>
        /// List of new events added during an execution to keep track of the new events that were added during a particular execution 
        /// should not be serialized
        /// </summary>
        public IList<HistoryEvent> NewEvents { get; }

        readonly ISet<int> completedEventIds;

        /// <summary>
        /// Compressed size of the serialized state
        /// </summary>
        public long CompressedSize;

        /// <summary>
        /// Gets the execution completed event
        /// </summary>
        public ExecutionCompletedEvent ExecutionCompletedEvent { get; set; }

        /// <summary>
        /// Size of the serialized state (uncompressed)
        /// </summary>
        public long Size;

        /// <summary>
        /// The string status of the runtime state
        /// </summary>
        public string Status;

        /// <summary>
        /// Creates a new instance of the OrchestrationRuntimeState
        /// </summary>
        public OrchestrationRuntimeState()
            : this(null)
        {
        }

        /// <summary>
        /// Creates a new instance of the OrchestrationRuntimeState with the supplied events
        /// </summary>
        /// <param name="events">List of events for this runtime state</param>
        public OrchestrationRuntimeState(IList<HistoryEvent> events)
        {
            Events = new List<HistoryEvent>();
            NewEvents = new List<HistoryEvent>();
            completedEventIds = new HashSet<int>();

            if (events != null && events.Count > 0)
            {
                foreach (HistoryEvent ev in events)
                {
                    AddEvent(ev, false);
                }
            }
        }

        /// <summary>
        /// Gets the execution started event
        /// </summary>
        public ExecutionStartedEvent ExecutionStartedEvent
        {
            get; private set;
        }

        /// <summary>
        /// Gets the OrchestrationInstance of the ExecutionStartedEvent else null
        /// </summary>
        public OrchestrationInstance OrchestrationInstance => ExecutionStartedEvent?.OrchestrationInstance;

        /// <summary>
        /// Adds a new history event to the Events list and optionally NewEvents list
        /// </summary>
        /// <param name="historyEvent">The history event to add</param>
        /// <param name="isNewEvent">Flag indicating whether this is a new event or not</param>
        void AddEvent(HistoryEvent historyEvent, bool isNewEvent)
        {
            if (IsDuplicateEvent(historyEvent))
            {
                return;
            }

            Events.Add(historyEvent);

            if (isNewEvent)
            {
                NewEvents.Add(historyEvent);
            }

            SetMarkerEvents(historyEvent);
        }

        bool IsDuplicateEvent(HistoryEvent historyEvent)
        {
            if (historyEvent.EventId >= 0 &&
                historyEvent.EventType == EventType.TaskCompleted &&
                !completedEventIds.Add(historyEvent.EventId))
            {
                TraceHelper.Trace(TraceEventType.Warning,
                    "OrchestrationRuntimeState-DuplicateEvent",
                    "The orchestration {0} has already seen a completed task with id {1}.",
                    this.OrchestrationInstance.InstanceId,
                    historyEvent.EventId);
                return true;
            }
            return false;
        }

        void SetMarkerEvents(HistoryEvent historyEvent)
        {
            if (historyEvent is ExecutionStartedEvent startedEvent)
            {
                if (ExecutionStartedEvent != null)
                {
                    throw new InvalidOperationException(
                        "Multiple ExecutionStartedEvent found, potential corruption in state storage");
                }

                ExecutionStartedEvent = startedEvent;
            }
            else if (historyEvent is ExecutionCompletedEvent completedEvent)
            {
                if (ExecutionCompletedEvent != null)
                {
                    throw new InvalidOperationException(
                        "Multiple ExecutionCompletedEvent found, potential corruption in state storage");
                }

                ExecutionCompletedEvent = completedEvent;
            }
        }
    }
}
