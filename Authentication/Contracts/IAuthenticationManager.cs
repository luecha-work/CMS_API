using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Authentication.DTOs;
using Entities;
using Microsoft.AspNetCore.Identity;
using Shared.DTOs.Account;

namespace Authentication.Contracts
{
    public interface IAuthenticationManager
    {
        Task<Account> FindAccountByEmail(string email);
        Task<Account> FindAccountByUsername(string username);
        Task<bool> CheckPassword(Account account, string password);
        Task<IList<string>> GetRolesForAccount(Account account);
        Task<IList<Claim>> GetClaimsForAccount(Account account);
        Task RemoveAuthenticationToken(
            Account account,
            string loginProvider,
            string refreshTokenProvider
        );
        Task<IdentityResult> SetAuthenticationToken(
            Account account,
            string loginProvider,
            string refreshTokenProvider,
            string newRefreshToken
        );
        Task<string> GenerateUserToken(
            Account account,
            string loginProvider,
            string refreshTokenProvider
        );
        Task<bool> VerifyUserToken(
            Account account,
            string loginProvider,
            string refreshTokenProvider,
            string refreshToken
        );
        Task<IdentityResult> UpdateSecurityStamp(Account account);

        void TestAuthRepo();
    }
}
