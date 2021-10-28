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
    using System.Threading.Tasks;
    using DurableTask.Core.Common;
    using DurableTask.Core.Exceptions;
    using DurableTask.Core.Serializing;

    /// <summary>
    /// Base class for TaskOrchestration
    ///     User activity should almost always derive from either 
    ///     TaskOrchestration&lt;TResult, TInput&gt; or 
    ///     TaskOrchestration&lt;TResult, TInput, TEvent, TStatus&gt;
    /// </summary>
    public abstract class TaskOrchestration
    {
        /// <summary>
        /// Abstract method for executing an orchestration based on the context and serialized input
        /// </summary>
        /// <param name="context">The orchestration context</param>
        /// <param name="input">The serialized input</param>
        /// <returns>Serialized output from the execution</returns>
        public abstract Task<string> Execute(OrchestrationContext context, string input);
    }
}
