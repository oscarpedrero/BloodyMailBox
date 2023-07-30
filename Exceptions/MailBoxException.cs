using System;
using System.Runtime.Serialization;

namespace BloodyMailBox.Exceptions
{
    [Serializable]
    internal class MailBoxException : Exception
    {

        public MailBoxException()
        {
        }

        public MailBoxException(string message)
                : base(message)
        {
        }

        public MailBoxException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MailBoxException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
