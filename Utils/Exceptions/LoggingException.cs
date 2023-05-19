using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Utils.Exceptions
{
    internal class LoggingException : Exception
    {
        public LoggingException()
        {
            Logging.Error("LoggingException message is null");
        }

        public LoggingException(string message)
            : base(message)
        {
            Logging.Error(message);
        }
    }
}
