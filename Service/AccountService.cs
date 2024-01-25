using AutoMapper;
using CMS_API.Core.Exceptions;
using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository.Contracts;
using Service.Contracts;
using Shared.DTOs.Account;
using Shared.Query;

namespace Service
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;
        private Account _user;

        public AccountService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            this._mapper = mapper;
            this._repositoryManager = repositoryManager;
        }

        public async Task<bool> AccountExists(string id) =>
            await _repositoryManager.accountRepository.Exists(id);

        public async Task<IEnumerable<IdentityError>> CreateAccount(CreateAccountDto userDto)
        {
            bool isDuplicate;
            bool isAdEmailDuplicate;
            bool isAdUsernameDuplicate;

            //TODO:Valisdate User to create
            if (userDto.UserLocal)
            {
                isDuplicate = await _repositoryManager
                    .accountRepository
                    .CheckUsernameForAccount(userDto.UserName);

                if (isDuplicate)
                {
                    throw new AccountWithUsernameDuplicateBadRequestException(userDto.UserName);
                }
            }
            else
            {
                isAdEmailDuplicate = await _repositoryManager
                    .accountRepository
                    .CheckEmailForAccount(userDto.Email, false);

                isAdUsernameDuplicate = await _repositoryManager
                    .accountRepository
                    .CheckUsernameForAccount(userDto.UserName);

                if (isAdEmailDuplicate)
                    throw new AccountWithEmailDuplicateBadRequestException(userDto.Email);

                if (isAdUsernameDuplicate)
                    throw new AccountWithUsernameDuplicateBadRequestException(userDto.UserName);
            }

            bool isFoundRole = await _repositoryManager
                .accountRepository
                .CheckRoleNameForAccount(userDto.RoleName);

            if (!isFoundRole)
                throw new RoleNameNotFoundException(userDto.RoleName);

            var account = await _repositoryManager.accountRepository.CreateAccount(userDto);

            _repositoryManager.Commit();

            return account;
        }

        public async Task DeleteAccount(string id)
        {
            var account = await _repositoryManager.accountRepository.FindAccountById(id);

            _repositoryManager.accountRepository.DeleteAsync(account);

            _repositoryManager.Commit();
        }

        public async Task<AccountDto> GetAccountById(string id)
        {
            var account = await _repositoryManager.accountRepository.FindAccountById(id);
            var accountResult = _mapper.Map<AccountDto>(account);

            return accountResult;
        }

        public async Task<List<AccountDto>> GetAllAccount()
        {
            var account = await _repositoryManager.accountRepository.GetAllAsync();
            var accountResult = _mapper.Map<List<AccountDto>>(account);

            return accountResult;
        }

        public async Task<PagedResult<AccountDto>> GetPagedAccount(QueryParameters queryParameters)
        {
            return await _repositoryManager
                .accountRepository
                .GetPagedResultAsync<AccountDto>(queryParameters);
        }

        public async Task UpdateAccount(UpdateAccountDto updateAccountDto)
        {
            var account = await _repositoryManager
                .accountRepository
                .FindAccountById(updateAccountDto.Id);

            _mapper.Map(updateAccountDto, account);

            await _repositoryManager.accountRepository.UpdateAsync(account);

            _repositoryManager.Commit();
        }
    }
}
