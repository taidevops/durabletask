namespace DurableTask.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using DurableTask.Core.Common;
    using DurableTask.Core.History;

    /// <summary>
    /// Represents the runtime state of an orchestration
    /// </summary>
    public class OrchestrationRuntimeState
    {
        /// <summary>
        /// Compressed size of the serialized state
        /// </summary>
        public long CompressedSize;
    }
}
