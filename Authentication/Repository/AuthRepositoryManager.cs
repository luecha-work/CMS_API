using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Contracts;
using Authentication.Models.ConfigurationModels;
using AutoMapper;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Authentication.Repository
{
    public class AuthRepositoryManager : IAuthRepositoryManager
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<Account> _userManager;
        private readonly IMapper _mapper;
        private readonly CMSDevDbContext _context;

        private readonly Lazy<IAuthenticationManager> _authenticationManager;
        private readonly Lazy<IAxonscmsSessionRepository> _axonscmsSessionRepository;

        public AuthRepositoryManager(
            CMSDevDbContext context,
            IConfiguration configuration,
            UserManager<Account> userManager,
            IMapper mapper
        )
        {
            this._context = context;
            this._mapper = mapper;
            this._userManager = userManager;
            this._configuration = configuration;

            _authenticationManager = new Lazy<IAuthenticationManager>(
                () => new AuthenticationManager(_userManager, _configuration, _mapper)
            );
            _axonscmsSessionRepository = new Lazy<IAxonscmsSessionRepository>(
                () => new AxonscmsSessionRepository(_context, _configuration, _mapper)
            );
        }

        public IAuthenticationManager authManager => _authenticationManager.Value;

        public IAxonscmsSessionRepository axonscmsSessionRepository =>
            _axonscmsSessionRepository.Value;

        public void Commit() => _context.SaveChanges();
    }
}
