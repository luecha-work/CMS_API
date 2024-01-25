using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Authentication.Contracts;
using Authentication.DTOs;
using AutoMapper;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Authentication.Repository
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<Account> _userManager;
        private readonly IConfiguration _configuration;
        private Account _user;

        public AuthenticationManager(
            UserManager<Account> userManager,
            IConfiguration configuration,
            IMapper mapper
        )
        {
            this._mapper = mapper;
            this._userManager = userManager;
            this._configuration = configuration;
        }

        public async Task<Account> FindAccountByEmail(string email) =>
            await _userManager.FindByEmailAsync(email);

        public async Task<Account> FindAccountByUsername(string username) =>
            await _userManager.FindByNameAsync(username);

        public async Task<bool> CheckPassword(Account account, string password) =>
            await _userManager.CheckPasswordAsync(account, password);

        public async Task<IList<string>> GetRolesForAccount(Account account) =>
            await _userManager.GetRolesAsync(account);

        public async Task<IList<Claim>> GetClaimsForAccount(Account account) =>
            await _userManager.GetClaimsAsync(account);

        public void TestAuthRepo() => Console.WriteLine("Hello Authentication Manager");

        public async Task RemoveAuthenticationToken(
            Account account,
            string loginProvider,
            string refreshTokenProvider
        )
        {
            await _userManager.RemoveAuthenticationTokenAsync(
                account,
                loginProvider,
                refreshTokenProvider
            );
        }

        public async Task<IdentityResult> SetAuthenticationToken(
            Account account,
            string loginProvider,
            string refreshTokenProvider,
            string newRefreshToken
        )
        {
            var result = await _userManager.SetAuthenticationTokenAsync(
                account,
                loginProvider,
                refreshTokenProvider,
                newRefreshToken
            );

            return result;
        }

        public async Task<string> GenerateUserToken(
            Account account,
            string loginProvider,
            string refreshTokenProvider
        )
        {
            var token = await _userManager.GenerateUserTokenAsync(
                account,
                loginProvider,
                refreshTokenProvider
            );

            return token;
        }

        public async Task<bool> VerifyUserToken(
            Account account,
            string loginProvider,
            string refreshTokenProvider,
            string refreshToken
        )
        {
            var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(
                account,
                loginProvider,
                refreshTokenProvider,
                refreshToken
            );

            return isValidRefreshToken;
        }

        public async Task<IdentityResult> UpdateSecurityStamp(Account account) =>
            await this._userManager.UpdateSecurityStampAsync(account);
    }
}
