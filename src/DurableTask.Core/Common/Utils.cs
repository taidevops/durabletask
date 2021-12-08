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

namespace DurableTask.Core.Common
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Runtime.ExceptionServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using DurableTask.Core.Exceptions;
    using DurableTask.Core.History;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Utility Methods
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Gets the version of the DurableTask.Core nuget package, which by convension is the same as the assembly file version.
        /// </summary>
        internal static readonly string PackageVersion = FileVersionInfo.GetVersionInfo(typeof(TaskOrchestration).Assembly.Location).FileVersion;

        /// <summary>
        /// Gets or sets the name of the app, for use when writing structured event source traces.
        /// </summary>
        /// <remarks>
        /// The default value comes from the WEBSITE_SITE_NAME environment variable, which is defined
        /// in Azure App Service. Other environments can use DTFX_APP_NAME to set this value.
        /// </remarks>
        public static string AppName { get; set; } =
            Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME") ??
            Environment.GetEnvironmentVariable("DTFX_APP_NAME") ??
            string.Empty;
    }
}
