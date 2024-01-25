using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication.DTOs;
using Authentication.Service.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CMS_API.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthServiceManager _authServiceManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;

        public AuthenticationController(
            IAuthServiceManager authServiceManager,
            IConfiguration configuration,
            ILogger<AccountController> logger
        )
        {
            this._authServiceManager = authServiceManager;
            this._configuration = configuration;
            this._logger = logger;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<string>>> GetTModel()
        {
            return Ok("hello");
        }

        [HttpPost("login-local")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            this._logger.LogInformation($"Loging Attempt for {loginDto.Username}");

            var authResponse = await this._authServiceManager.AuthenticationService.Login(loginDto);

            if (authResponse == null)
            {
                return Unauthorized();
            }

            return Ok(authResponse);
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RefreshToken([FromBody] AuthResponseDto request)
        {
            var authResponse = await _authServiceManager
                .AuthenticationService
                .VerifyRefreshToken(request);

            if (authResponse == null)
            {
                return Unauthorized();
            }

            return Ok(authResponse);
        }
    }
}
