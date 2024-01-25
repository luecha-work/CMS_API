using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Service.Contracts
{
    public interface IAuthServiceManager
    {
        IAuthenticationService AuthenticationService { get; }
        IAzureAdTokenService AzureAdTokenService { get; }
    }
}
