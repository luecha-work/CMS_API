using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using Shared.DTOs.Account;

namespace Repository.EntityFramework
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        private readonly IMapper _mapper;
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<Roles> _roleManager;
        private Account _user;

        public AccountRepository(
            CMSDevDbContext context,
            IMapper mapper,
            UserManager<Account> userManager,
            RoleManager<Roles> roleManager
        )
            : base(context, mapper)
        {
            this._mapper = mapper;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task<IEnumerable<IdentityError>> CreateAccount(CreateAccountDto userDto)
        {
            _user = _mapper.Map<Account>(userDto);
            // _user.UserName = userDto.UserName;

            var result = await _userManager.CreateAsync(_user, userDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, userDto.RoleName);
            }

            return result.Errors;
        }

        public async Task<bool> CheckPassword(Account account, string password) =>
            await _userManager.CheckPasswordAsync(account, password);

        public async Task<Account> FindAccountById(string id) =>
            await _userManager.FindByIdAsync(id);

        public async Task<Account> FindAccountByEmail(string email) =>
            await _userManager.FindByEmailAsync(email);

        public async Task<Account> FindAccountByUsername(string username) =>
            await _userManager.FindByNameAsync(username);

        public async Task<bool> CheckUsernameForAccount(string username)
        {
            bool isDuplicate = false;

            var user = await _userManager
                .Users
                .Where(u => u.UserName == username)
                .FirstOrDefaultAsync();

            isDuplicate = (user == null) ? false : true;

            return isDuplicate;
        }

        public async Task<bool> CheckEmailForAccount(string email, bool userLocal)
        {
            bool isDuplicate = false;

            var user = await _userManager
                .Users
                .Where(u => u.Email == email && u.UserLocal == userLocal)
                .FirstOrDefaultAsync();

            isDuplicate = (user == null) ? false : true;

            return isDuplicate;
        }

        public async Task<bool> CheckRoleNameForAccount(string rolename) =>
            await _roleManager.RoleExistsAsync(rolename);
    }
}
