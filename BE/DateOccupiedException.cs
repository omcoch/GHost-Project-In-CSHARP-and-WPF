using System;
using System.Runtime.Serialization;

namespace BE
{
    [Serializable]
    public class DateOccupiedException : Exception
    {
        public DateOccupiedException()
        {
        }

        public DateOccupiedException(string message) : base(message)
        {
        }

        public DateOccupiedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DateOccupiedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}