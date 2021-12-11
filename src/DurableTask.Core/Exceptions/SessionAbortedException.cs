using System;
using System.Collections.Generic;
using System.Text;

namespace DurableTask.Core.Exceptions
{
    /// <summary>
    /// Thrown when an orchestration or activity session is aborted, for example in split-brain situation or host shutdown situations.
    /// </summary>
    [Serializable]
    public class SessionAbortedException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionAbortedException"/> class.
        /// </summary>
        public SessionAbortedException()
            : base("The current execution has been aborted.")
        {
        }

        /// <summary>
        /// Initializes an new instance of the <see cref="SessionAbortedException"/> class with a specified error message
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SessionAbortedException(string message)
            : base(message)
        {
        }
    }
}
