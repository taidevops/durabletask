using System;
using System.Collections.Generic;
using System.Text;

namespace DurableTask.Core
{
    /// <summary>
    /// A snapshot / state dump of an OrchestrationRuntimeState's events
    /// </summary>
    public class OrchestrationRuntimeStateDump
    {
        /// <summary>
        /// The number of all history events for this runtime state dump.
        /// </summary>
        public int EventCount { get; set; }
    }
}
