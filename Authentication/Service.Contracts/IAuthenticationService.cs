using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication.DTOs;
using Entities;

namespace Authentication.Service.Contracts
{
    public interface IAuthenticationService
    {
        Task<AuthResponseDto?> Login(LoginDto loginDto);
        Task<string> CreateRefreshToken();
        Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request);
        Task CreateAxonscmsSession(LoginDto loginDto);
        Task UpdateAxonscmsSession();
    }
}
