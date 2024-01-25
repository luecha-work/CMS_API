using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Models.ConfigurationModels;
using Authentication.Service.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Authentication.Service
{
    public class AuthServiceManager : IAuthServiceManager
    {
        private readonly Lazy<IAuthenticationService> _authenticationService;

        public AuthServiceManager(
            Authentication.Contracts.IAuthRepositoryManager authenticationManager,
            IConfiguration configuration,
            IOptions<JwtConfiguration> configurationJWT,
            IOptions<IdentityProviderConfigure> configurationIdentityConfigure,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor
        // ILogger<AuthServiceManager> logger,
        // ILoggerManager logger
        )
        {
            _authenticationService = new Lazy<IAuthenticationService>(
                () =>
                    new AuthenticationService(
                        authenticationManager,
                        configuration,
                        configurationJWT,
                        configurationIdentityConfigure,
                        httpContextAccessor,
                        mapper
                    )
            );

            // _azureAdTokenService = new Lazy<IAzureAdTokenService>(
            //     () => new AzureAdTokenService(configuration)
            // );
        }

        public IAuthenticationService AuthenticationService => _authenticationService.Value;

        public IAzureAdTokenService AzureAdTokenService => throw new NotImplementedException();
    }
}
