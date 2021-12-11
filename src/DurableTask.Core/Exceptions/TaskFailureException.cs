namespace DurableTask.Core.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception type thrown by implementors of <see cref="TaskActivity"/> when exception
    /// details need to flow to parent orchestrations.
    /// </summary>
    [Serializable]
    public class TaskFailureException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskFailureException"/> class.
        /// </summary>
        public TaskFailureException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskFailureException"/> class.
        /// </summary>
        public TaskFailureException(string reason)
            : base(reason)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskFailureException"/> class.
        /// </summary>
        public TaskFailureException(string reason, Exception innerException, string details)
            : base(reason, innerException)
        {
            Details = details;
        }

        /// <summary>
        /// Details of the exception which will flow to the parent orchestration.
        /// </summary>
        public string Details { get; set; }
    }
}
