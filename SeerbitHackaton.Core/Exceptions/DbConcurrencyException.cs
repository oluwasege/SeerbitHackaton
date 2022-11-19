global using System.Runtime.Serialization;

namespace SeerbitHackaton.Core.Exceptions
{
    [Serializable]
    public class AppDbConcurrencyException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="AbpDbConcurrencyException"/> object.
        /// </summary>
        public AppDbConcurrencyException()
        {
        }

        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// </summary>
        public AppDbConcurrencyException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        /// <summary>
        /// Creates a new <see cref="AbpDbConcurrencyException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public AppDbConcurrencyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new <see cref="AbpDbConcurrencyException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public AppDbConcurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}