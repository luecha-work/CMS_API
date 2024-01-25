using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IServiceManager
    {
        #region UserAccount
        IAccountService AccountService { get; }

        #endregion
    }
}
