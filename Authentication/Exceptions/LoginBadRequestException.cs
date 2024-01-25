using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS_API.Core.Exceptions;

namespace Authentication.Exceptions
{
    public class LoginBadRequestException : BadRequestException
    {
        public LoginBadRequestException(string message)
            : base(message) { }
    }
}
