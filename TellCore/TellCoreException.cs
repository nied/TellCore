using System;
using System.Runtime.Serialization;

namespace TellCore
{
    [Serializable]
    public class TellCoreException : InvalidOperationException
    {
        public TellCoreException() { }
        public TellCoreException(string message) : base(message) { }
        public TellCoreException(string message, Exception innerException) : base(message, innerException) { }
        public TellCoreException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
