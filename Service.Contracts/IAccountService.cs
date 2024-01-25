using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Account;
using Shared.Query;

namespace Service.Contracts
{
    public interface IAccountService
    {
        // (IEnumerable<AccountDto> accounts, MetaData metaData) GetListAccount(
        //     AccountParameters parameters,
        //     bool trackChanges
        // );

        Task<AccountDto> GetAccountById(string id);

        Task<List<AccountDto>> GetAllAccount();

        Task<PagedResult<AccountDto>> GetPagedAccount(QueryParameters queryParameters);

        Task<IEnumerable<IdentityError>> CreateAccount(CreateAccountDto userDto);

        Task UpdateAccount(UpdateAccountDto updateAccountDto);

        Task<bool> AccountExists(string id);

        Task DeleteAccount(string id);

        // void DeleteAccount(int id);
        // void DeleteAccountCollection(int[] ids);
        // AccountDto GetAccount(int id, bool trackChanges);

        // void UpdateLanguage(int id, AccountForLanguageUpdateDto account);
    }
}
