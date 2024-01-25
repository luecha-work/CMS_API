using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Identity;
using Shared.DTOs.Account;

namespace Repository.Contracts
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<Account> FindAccountByEmail(string email);
        Task<Account> FindAccountByUsername(string username);
        Task<Account> FindAccountById(string id);
        Task<bool> CheckPassword(Account account, string password);
        Task<IEnumerable<IdentityError>> CreateAccount(CreateAccountDto userDto);
        Task<bool> CheckUsernameForAccount(string username);
        Task<bool> CheckEmailForAccount(string email, bool userLocal);
        Task<bool> CheckRoleNameForAccount(string rolename);
    }
}
