using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Contracts
{
    public interface IAuthRepositoryManager
    {
        // IUserManagerRepository UserManager { get; }
        IAuthenticationManager authManager { get; }
        IAxonscmsSessionRepository axonscmsSessionRepository { get; }
        void Commit();
    }
}
