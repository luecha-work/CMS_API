using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CMS_API.Core.Exceptions
{
    [Serializable]
    public sealed class RoleNameNotFoundException : NotFoundException
    {
        public RoleNameNotFoundException(string rolename)
            : base($"Role with RoleName: {rolename} doesn't exist in the database.") { }
    }

    [Serializable]
    public sealed class RoleCodeNotFoundException : NotFoundException
    {
        public RoleCodeNotFoundException(string rolecode)
            : base($"Role with RoleCode: {rolecode} doesn't exist in the database.") { }
    }
}
