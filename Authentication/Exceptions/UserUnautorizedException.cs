using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Authentication.Exceptions;

namespace Authentication.Exceptions
{
    [Serializable]
    public sealed class UserUnautorizedException : UnauthorizedException
    {
        public UserUnautorizedException(string message)
            : base(message) { }

        private UserUnautorizedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
