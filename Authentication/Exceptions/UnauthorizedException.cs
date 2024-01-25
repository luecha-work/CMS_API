using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Exceptions
{
    [Serializable]
    public abstract class UnauthorizedException : Exception
    {
        protected UnauthorizedException(string message)
            : base(message) { }

        protected UnauthorizedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
