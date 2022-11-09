using System;
using System.Collections.Generic;
using System.Text;

#nullable enable
namespace DurableTask.Core.Command
{
    using Newtonsoft.Json;


    [JsonConverter(typeof())]
    public abstract class OrchestratorAction
    {
    }
}
