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
    using System.Threading;
    using System.Threading.Tasks;
    using DurableTask.Core.Serializing;
    using ImpromptuInterface;

    /// <summary>
    /// Context for an orchestration containing the instance, replay status, orchestration methods and proxy methods
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
        /// JsonDataConverter for message serialization settings
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
    }
}
