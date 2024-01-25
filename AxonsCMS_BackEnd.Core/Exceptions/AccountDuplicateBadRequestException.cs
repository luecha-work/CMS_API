using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CMS_API.Core.Exceptions
{
    [Serializable]
    public sealed class AccountWithUsernameDuplicateBadRequestException : BadRequestException
    {
        public AccountWithUsernameDuplicateBadRequestException(string username)
            : base($"Duplicate Account with Username: {username}.") { }
    }

    [Serializable]
    public sealed class AccountWithEmailDuplicateBadRequestException : BadRequestException
    {
        public AccountWithEmailDuplicateBadRequestException(string email)
            : base($"Duplicate Account with Email: {email}.") { }
    }
}
