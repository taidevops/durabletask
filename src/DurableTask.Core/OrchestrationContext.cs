using DurableTask.Core.Serializing;
using System;
using System.Collections.Generic;
using System.Text;

namespace DurableTask.Core
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class OrchestrationContext
    {
        /// <summary>
        /// Thread-static variable used to signal whether the calling thread is the orchestrator thread.
        /// The primary use case is for detecting illegal async usage in orchestration code.
        /// </summary>
        [ThreadStatic]
        public static bool IsOrchestratorThread;

        /// <summary>
        /// JsonDataConverter for error serialization settings
        /// </summary>
        public JsonDataConverter MessageDataConverter { get; set; }

        /// <summary>
        /// JsonDataConverter for error serialization settings
        /// </summary>
        public JsonDataConverter ErrorDataConverter { get; set; }

        /// <summary>
        /// Instance of the currently executing orchestration
        /// </summary>
        public OrchestrationInstance OrchestrationInstance { get; internal protected set; }

        /// <summary>
        /// Replay-safe current UTC datetime
        /// </summary>
        public virtual DateTime CurrentUtcDateTime { get; internal set; }

        /// <summary>
        ///     True if the code it currently replaying, False if code is truly executing for the first time.
        /// </summary>
        public bool IsReplaying { get; internal protected set; }
    }
}
