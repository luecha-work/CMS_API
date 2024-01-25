using Repository.Contracts;

namespace CMS_API.ContextFactory
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public RepositoryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IRepositoryManager Create(RepoType repoType)
        {
            return repoType switch
            {
                RepoType.EntityFramework
                    => _serviceProvider.GetRequiredService<Repository.EntityFramework.RepositoryManager>(),

                // RepoType.Dapper
                //     => _serviceProvider.GetRequiredService<Repository.RepositoryManager>(),
                // _ => throw new NotImplementedException(),
            };
        }
    }
}
