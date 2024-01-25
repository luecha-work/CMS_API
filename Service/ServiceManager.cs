using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Repository.Contracts;
using Service.Contracts;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAccountService> _accountService;

        public ServiceManager(
            IRepositoryFactory repositoryFactory,
            IMapper mapper
        // UserManager<Account> userProvider,
        // IOptions<SmtpConfiguration> configurationSmpt,
        // IOptions<AwsS3Configuration> configurationS3,
        // IConfiguration configuration,
        // IUserProvider userProvider,
        // ILoggerManager logger,
        // IElasticSearchManager elasticSearchManager,
        // IOptions<PowerBIConfiguration> configurationPowerBI,
        // IOptions<AzureOpenAIConfiguration> configurationAzureOpenAI,
        // OpenAIClient openAIClient,
        // IOptions<GoogleVisionConfiguration> configurationGoogleVision
        )
        {
            // var _repositoryDPManager = repositoryFactory.Create(RepoType.Dapper);
            var _repositoryEFManager = repositoryFactory.Create(RepoType.EntityFramework);

            _accountService = new Lazy<IAccountService>(
                () => new AccountService(_repositoryEFManager, mapper)
            );
        }

        #region UserAccount
        public IAccountService AccountService => _accountService.Value;

        #endregion UserAccount
    }
}
