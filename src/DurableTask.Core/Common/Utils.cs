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
    using DurableTask.Core.Exceptions;
    using DurableTask.Core.Serializing;
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

        internal static JArray ConvertToJArray(string input)
        {
            JArray jArray;
            using (var stringReader = new StringReader(input))
            using (var jsonTextReader = new JsonTextReader(stringReader) { DateParseHandling = DateParseHandling.None })
            {
                jArray = JArray.Load(jsonTextReader);
            }

            return jArray;
        }

        /// <summary>
        /// Returns true or false whether an exception is considered fatal
        /// </summary>
        public static bool IsFatal(Exception exception)
        {
            if (exception is OutOfMemoryException || exception is StackOverflowException)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if an exception represents an aborting execution; false otherwise.
        /// </summary>
        public static bool IsExecutionAborting(Exception exception) => exception is SessionAbortedException;

        /// <summary>
        /// Serializes the supplied exception to a string
        /// </summary>
        public static string SerializeCause(Exception originalException, DataConverter converter)
        {
            if (originalException == null)
            {
                throw new ArgumentNullException(nameof(originalException));
            }

            if (converter == null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            string details;
            try
            {
                details = converter.Serialize(originalException);
            }
            catch
            {
                // Cannot serialize exception, throw original exception
                ExceptionDispatchInfo.Capture(originalException).Throw();
                throw originalException; // no op
            }

            return details;
        }
    }
}
