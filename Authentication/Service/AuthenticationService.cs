using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Authentication.DTOs;
using Authentication.Exceptions;
using Authentication.Models.ConfigurationModels;
using Authentication.Service.Contracts;
using AutoMapper;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly Authentication.Contracts.IAuthRepositoryManager _repositoryManager;
        private readonly IHttpContextAccessor? _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly IdentityProviderConfigure _configurationIdentityProvider;
        private readonly IMapper _mapper;

        private Account _user;
        private AxonscmsSession _cmsSession;

        public AuthenticationService(
            Authentication.Contracts.IAuthRepositoryManager repositoryManager,
            IConfiguration configuration,
            IOptions<JwtConfiguration> configurationJWT,
            IOptions<IdentityProviderConfigure> configurationIdentityConfigure,
            IHttpContextAccessor? httpContextAccessor,
            IMapper mapper
        )
        {
            this._repositoryManager = repositoryManager;
            this._configuration = configuration;
            this._jwtConfiguration = configurationJWT.Value;
            this._configurationIdentityProvider = configurationIdentityConfigure.Value;
            this._httpContextAccessor = httpContextAccessor;
            this._mapper = mapper;
        }

        public async Task<AuthResponseDto?> Login(LoginDto loginDto)
        {
            // _logger.LogWarning($"Looking for user with email {loginDto.Email}");
            Console.WriteLine($"Hello Login Service => {loginDto.Username}");

            _user = await _repositoryManager.authManager.FindAccountByUsername(loginDto.Username);

            bool isValidUser = await _repositoryManager
                .authManager
                .CheckPassword(_user, loginDto.Password);

            if (_user == null)
                throw new LoginBadRequestException("Please enter a valid username.");

            if (!isValidUser)
                throw new LoginBadRequestException("Please enter a valid password.");

            await CreateAxonscmsSession(loginDto);

            var token = await GenerateToken();

            _cmsSession.Token = token;

            await UpdateAxonscmsSession();

            return new AuthResponseDto
            {
                AccessToken = token,
                UserId = _user.Id,
                RefreshToken = await CreateRefreshToken()
            };
        }

        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.AccessToken);

            var username = tokenContent
                .Claims
                .ToList()
                .FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Sub)
                ?.Value;

            // var username = tokenContent.Claims.FirstOrDefault(q => q.Type == "username")?.Value;
            var sessionId = tokenContent.Claims.FirstOrDefault(q => q.Type == "session_id")?.Value;

            _user = await _repositoryManager.authManager.FindAccountByUsername(username);
            _cmsSession = await _repositoryManager
                .axonscmsSessionRepository
                .FindSessionById(sessionId);

            if (_user == null || _user.Id != request.UserId)
            {
                return null;
            }

            var isValidRefreshToken = await _repositoryManager
                .authManager
                .VerifyUserToken(
                    _user,
                    _configurationIdentityProvider.LoginProvider,
                    _configurationIdentityProvider.RefreshTokenProvider,
                    request.RefreshToken
                );

            if (isValidRefreshToken)
            {
                var token = await GenerateToken();

                _cmsSession.Token = token;

                await UpdateAxonscmsSession();

                return new AuthResponseDto
                {
                    AccessToken = token,
                    UserId = _user.Id,
                    RefreshToken = request.RefreshToken
                };
            }

            await _repositoryManager.authManager.UpdateSecurityStamp(_user);
            _repositoryManager.Commit();

            return null;
        }

        private async Task<string> GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey) //TODO: Get Key from JwtSettings in  applications.json
            );

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _repositoryManager.authManager.GetRolesForAccount(_user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await _repositoryManager.authManager.GetClaimsForAccount(_user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                // new Claim("username", _user.UserName),
                new Claim("session_id", _cmsSession.SessionId.ToString()),
                new Claim("account_id", _user.Id),
            }
                .Union(userClaims)
                .Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _jwtConfiguration.ValidIssuer,
                audience: _jwtConfiguration.ValidAudience,
                claims: claims,
                expires: DateTime
                    .Now
                    .AddMinutes(Convert.ToInt32(_jwtConfiguration.DurationInMinutes)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateRefreshToken()
        {
            if (_user != null)
            {
                await _repositoryManager
                    .authManager
                    .RemoveAuthenticationToken(
                        _user,
                        _configurationIdentityProvider.LoginProvider,
                        _configurationIdentityProvider.RefreshTokenProvider
                    );

                var newRefreshToken = await _repositoryManager
                    .authManager
                    .GenerateUserToken(
                        _user,
                        _configurationIdentityProvider.LoginProvider,
                        _configurationIdentityProvider.RefreshTokenProvider
                    );

                var result = await _repositoryManager
                    .authManager
                    .SetAuthenticationToken(
                        _user,
                        _configurationIdentityProvider.LoginProvider,
                        _configurationIdentityProvider.RefreshTokenProvider,
                        newRefreshToken
                    );

                _repositoryManager.Commit();

                return newRefreshToken;
            }
            else
            {
                Console.WriteLine("User Account is null. Cannot create refresh token.");

                return null;
            }
        }

        public async Task CreateAxonscmsSession(LoginDto loginDto)
        {
            var ipAddr = _httpContextAccessor?.HttpContext.Connection.RemoteIpAddress.ToString();
            var dateNow = DateTime.UtcNow;
            var dateExpiration = dateNow.AddHours(24);

            var oldSession = _repositoryManager
                .axonscmsSessionRepository
                .FindByCondition(
                    session => session.AccountId == _user.Id && session.LoginIp == ipAddr
                )
                .FirstOrDefault();

            var newSession = new AxonscmsSession()
            {
                AccountId = _user.Id,
                Browser = loginDto.Browser,
                Os = loginDto.Os,
                Platform = loginDto.PlatForm,
                LoginIp = ipAddr ?? "",
                SessionStatus = "A",
                IssuedTime = dateNow,
                ExpirationTime = dateExpiration,
                LoginAt = dateNow,
                CreateBy = "System",
                UpdateBy = "",
                CreateAt = dateNow,
                UpdateAt = dateNow,
            };

            _cmsSession = await _repositoryManager
                .axonscmsSessionRepository
                .CreateSession(newSession);

            if (oldSession != null)
            {
                await _repositoryManager.axonscmsSessionRepository.DeleteSession(oldSession);
            }

            _repositoryManager.Commit();
        }

        public async Task UpdateAxonscmsSession() =>
            await _repositoryManager.axonscmsSessionRepository.UpdatSession(_cmsSession);
    }
}
