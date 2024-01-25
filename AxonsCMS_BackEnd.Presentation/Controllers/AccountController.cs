using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CMS_API.Core.ErrorModel;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Contracts;
using Shared.DTOs.Account;
using Shared.Query;

namespace CMS_API.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IServiceManager _service;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;

        public AccountController(
            IServiceManager service,
            ILogger<AccountController> logger,
            IMapper mapper
        )
        {
            this._service = service;
            this._logger = logger;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<ActionResult<PagedResult<AccountDto>>> GetPagedAccount(
            [FromQuery] QueryParameters queryParameters
        )
        {
            var pagedAccountResult = await this._service
                .AccountService
                .GetPagedAccount(queryParameters);

            return Ok(pagedAccountResult);
        }

        [HttpGet("get-all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
        // [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetPagedCountries()
        {
            var account = await _service.AccountService.GetAllAccount();

            return Ok(account);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
        // [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<AccountDto>> GetAccountById(string id)
        {
            _logger.LogWarning($"Record found in {nameof(GetAccountById)}. with Id: {id}. ");

            var account = await _service.AccountService.GetAccountById(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
        // [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> CreateAccountAsync([FromBody] CreateAccountDto accountDto)
        {
            _logger.LogInformation($"CreateAccount Attemt for {accountDto.Email}");

            var errors = await this._service.AccountService.CreateAccount(accountDto);

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            return Ok(errors);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> UpdateAccount(string id, UpdateAccountDto updateAccountDto)
        {
            try
            {
                if (id != updateAccountDto.Id)
                    return BadRequest("Invalid Record Id");

                var account = await _service.AccountService.GetAccountById(id);

                if (account == null)
                    return NotFound();

                await _service.AccountService.UpdateAccount(updateAccountDto);

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
        [Authorize("Administrator")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var account = await _service.AccountService.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }

            await _service.AccountService.DeleteAccount(id);

            return NoContent();
        }

        private async Task<bool> AccountExists(string id)
        {
            return await _service.AccountService.AccountExists(id);
        }
    }
}
