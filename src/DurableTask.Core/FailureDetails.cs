using System;
using System.Collections.Generic;
using System.Text;

#nullable enable
namespace DurableTask.Core
{
    using System;
    using System.Runtime.Serialization;
    using DurableTask.Core.Exceptions;
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class FailureDetails
    {
        public string ErrorType { get; }

        public string ErrorMessage { get; }

        public string? StackTrace { get; }

        public FailureDetails? InnerFailure { get; }

        public bool IsNonRetriable { get; }

        public override string ToString()
        {
            return $"{this.ErrorType}: {this.ErrorMessage}";
        }

        static string GetErrorMessage(Exception e)
        {
            if (e is )
        }
    }
}
