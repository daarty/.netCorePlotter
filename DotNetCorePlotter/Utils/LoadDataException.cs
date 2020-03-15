using System;
using System.Runtime.Serialization;

namespace DotNetCorePlotter.Utils
{
    [Serializable]
    public class LoadDataException : Exception
    {
        public LoadDataException()
        {
        }

        public LoadDataException(string message) : base(message)
        {
        }

        public LoadDataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        private LoadDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}