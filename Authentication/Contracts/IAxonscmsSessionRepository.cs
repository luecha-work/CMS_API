using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Entities;

namespace Authentication.Contracts
{
    public interface IAxonscmsSessionRepository
    {
        Task<AxonscmsSession> FindSessionById(string id);
        Task<AxonscmsSession> CreateSession(AxonscmsSession entity);
        Task UpdatSession(AxonscmsSession entity);
        Task DeleteSession(AxonscmsSession entity);
        IQueryable<AxonscmsSession> FindByCondition(
            Expression<Func<AxonscmsSession, bool>> expression
        );
    }
}
