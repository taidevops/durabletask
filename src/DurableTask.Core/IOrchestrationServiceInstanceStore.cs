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
    using System.Threading.Tasks;
    using DurableTask.Core.Tracking;

    /// <summary>
    /// Instance Store provider interface to allow storage and lookup for orchestration state and event history
    /// </summary>
    public interface IOrchestrationServiceInstanceStore
    {
        /// <summary>
        /// Gets the maximum length a history entry can be so it can be truncated if necessary
        /// </summary>
        /// <returns>The maximum length</returns>
        int MaxHistoryEntryLength { get; }

        /// <summary>
        /// Runs initialization to prepare the instance store for use
        /// </summary>
        /// <param name="recreate">Flag to indicate whether the store should be recreated.</param>
        Task InitializeStoreAsync(bool recreate);

        /// <summary>
        /// Deletes instances instance store
        /// </summary>
        Task DeleteStoreAsync();

        /// <summary>
        /// Writes a list of history events to instance store
        /// </summary>
        /// <param name="entities">List of history events to write</param>
        Task<object> WriteEntitiesAsync(IEnumerable<InstanceEntityBase> entities);

        /// <summary>
        /// Get a list of state events from instance store
        /// </summary>
        /// <param name="instanceId">The instance id to return state for</param>
        /// <param name="executionId">The execution id to return state for</param>
        /// <returns>The matching orchestration state or null if not found</returns>
        Task<IEnumerable<OrchestrationStateInstanceEntity>> GetEntitiesAsync(string instanceId, string executionId);

        /// <summary>
        /// Deletes a list of history events from instance store
        /// </summary>
        /// <param name="entities">List of history events to delete</param>
        Task<object> DeleteEntitiesAsync(IEnumerable<InstanceEntityBase> entities);
    }
}
