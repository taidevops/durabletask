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
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// TraceContext keep the correlation value.
    /// </summary>
    public abstract class TraceContextBase
    {
        protected TraceContextBase()
        {

        }

        static TraceContextBase()
        {

        }

        /// <summary>
        /// Start time of this telemetry
        /// </summary>
        public DateTimeOffset StartTime { get; set; }

        public
    }

    /// <summary>
    /// Telemetry Type
    /// </summary>
    public enum TelemetryType
    {
        /// <summary>
        /// Request Telemetry
        /// </summary>
        Request,

        /// <summary>
        /// Dependency Telemetry
        /// </summary>
        Dependency,
    }
}
