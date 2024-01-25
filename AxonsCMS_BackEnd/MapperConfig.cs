using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Entities;
using Shared.DTOs.Account;

namespace CMS_API
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            #region CreateMapperAccount
            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<Account, CreateAccountDto>().ReverseMap();
            CreateMap<Account, UpdateAccountDto>().ReverseMap();

            #endregion CreateMapperAccount
        }
    }
}
