using System;
using System.Runtime.Serialization;

namespace BE
{
    [Serializable]
    public class ExecutionOrderException : Exception
    {
        public ExecutionOrderException()
        {
        }

        public ExecutionOrderException(string message) : base(message)
        {
        }

        public ExecutionOrderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExecutionOrderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}