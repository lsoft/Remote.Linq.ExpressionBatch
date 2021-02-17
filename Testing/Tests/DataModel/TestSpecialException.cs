using System;
using System.Runtime.Serialization;

namespace Tests.DataModel
{
    public class TestSpecialException : Exception
    {
        /// <inheritdoc />
        public TestSpecialException()
        {
        }

        /// <inheritdoc />
        protected TestSpecialException(
            SerializationInfo info,
            StreamingContext context
            )
            : base(info, context)
        {
        }

        /// <inheritdoc />
        public TestSpecialException(
            string? message
            )
            : base(message)
        {
        }

        /// <inheritdoc />
        public TestSpecialException(
            string? message,
            Exception? innerException
            )
            : base(message, innerException)
        {
        }
    }
}
