// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace DurableTask.SqlServer
{
    using System;
    using System.Diagnostics;
    using DurableTask.Core;
    using DurableTask.Core.Logging;
    using Microsoft.Extensions.Logging;

    class LogHelper
    {
        readonly ILogger log;

        public LogHelper(ILogger log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }
    }
}
