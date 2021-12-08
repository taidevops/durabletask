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

    /// <summary>
    /// Base class for TaskOrchestration
    ///     User activity should almost always derive from either 
    ///     TaskOrchestration&lt;TResult, TInput&gt; or 
    ///     TaskOrchestration&lt;TResult, TInput, TEvent, TStatus&gt;
    /// </summary>
    public abstract class TaskOrchestration
    {
        
    }

    /// <summary>
    /// Typed base class for task orchestration
    /// </summary>
    /// <typeparam name="TResult">Output type of the orchestration</typeparam>
    /// <typeparam name="TInput">Input type for the orchestration</typeparam>
    public abstract class TaskOrchestration<TResult, TInput> : TaskOrchestration<TResult, TInput, string, string>
    {
    }

    /// <summary>
    /// Typed base class for Task orchestration with typed events and status
    /// </summary>
    /// <typeparam name="TResult">Output type of the orchestration</typeparam>
    /// <typeparam name="TInput">Input type for the orchestration</typeparam>
    /// <typeparam name="TEvent">Input type for RaiseEvent calls</typeparam>
    /// <typeparam name="TStatus">Output Type for GetStatus calls</typeparam>
    public abstract class TaskOrchestration<TResult, TInput, TEvent, TStatus> : TaskOrchestration
    {

    }
}
