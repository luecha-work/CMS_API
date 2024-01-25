using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Authentication.Contracts;
using AutoMapper;
using Entities;
using Microsoft.Extensions.Configuration;

namespace Authentication.Repository
{
    public class AxonscmsSessionRepository : IAxonscmsSessionRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly CMSDevDbContext _context;

        public AxonscmsSessionRepository(
            CMSDevDbContext context,
            IConfiguration configuration,
            IMapper mapper
        )
        {
            this._context = context;
            this._mapper = mapper;
            this._configuration = configuration;
        }

        public async Task<AxonscmsSession> CreateSession(AxonscmsSession entity)
        {
            var sessionEntry = await _context.AxonscmsSessions.AddAsync(entity);

            return sessionEntry.Entity;
        }

        public async Task DeleteSession(AxonscmsSession entity)
        {
            _context.AxonscmsSessions.Remove(entity);

            await _context.SaveChangesAsync();
        }

        public async Task<AxonscmsSession> FindSessionById(string id)
        {
            Guid sessionId;
            Guid.TryParse(id, out sessionId);

            var session = await _context.AxonscmsSessions.FindAsync(sessionId);
            var axonscmsSessionResult = _mapper.Map<AxonscmsSession>(session);

            return axonscmsSessionResult;
        }

        public async Task UpdatSession(AxonscmsSession entity)
        {
            _context.AxonscmsSessions.Update(entity);

            await this._context.SaveChangesAsync();
        }

        public IQueryable<AxonscmsSession> FindByCondition(
            Expression<Func<AxonscmsSession, bool>> expression
        )
        {
            return _context.AxonscmsSessions.Where(expression);
        }
    }
}
