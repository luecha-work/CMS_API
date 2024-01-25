using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Entities;
using Microsoft.AspNetCore.Identity;
using Repository.Contracts;

namespace Repository.EntityFramework
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly CMSDevDbContext _context;

        private readonly Lazy<IAccountRepository> _accountRepository;

        public RepositoryManager(
            CMSDevDbContext context,
            IMapper mapper,
            UserManager<Account> userManager,
            RoleManager<Roles> roleManager
        )
        {
            this._context = context;

            _accountRepository = new Lazy<IAccountRepository>(
                () => new AccountRepository(context, mapper, userManager, roleManager)
            );
        }

        public IAccountRepository accountRepository => _accountRepository.Value;

        public void Commit() => _context.SaveChanges();
    }
}
