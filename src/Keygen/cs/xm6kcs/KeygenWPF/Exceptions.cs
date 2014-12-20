using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeygenWPF
{

    public class MaximumUsageException : System.Exception
    {
        public MaximumUsageException() : base() { }
        public MaximumUsageException(String message) : base(message) { }
        public MaximumUsageException(String message, System.Exception innerException) : base(message, innerException) { }
    }

    public class ConnectionExeption : System.Exception
    {
        public ConnectionExeption() : base() { }
        public ConnectionExeption(String message) : base(message) { }
        public ConnectionExeption(String message, Exception innerException) : base(message, innerException) { }
    }
}
