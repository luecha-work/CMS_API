// using System;
// using System.Collections.Generic;
// using System.Data;
// using System.Linq;
// using System.Threading.Tasks;
// using AutoMapper;
// using Microsoft.Extensions.Configuration;
// using Npgsql;
// using Repository.Contracts;

// namespace Repository
// {
//     public class RepositoryManager : IRepositoryManager, IDisposable
//     {
//         private readonly IDbConnection _connection;
//         private readonly IMapper _mapper;
//         private IDbTransaction _transaction;

//         private IAccountRepository? _accountRepository;

//         private bool _disposed;

//         public RepositoryManager(IConfiguration configuration)
//         {
//             string connectionString = configuration.GetConnectionString("DefaultConnection");
//             _connection = new NpgsqlConnection(connectionString);
//             _connection.Open();
//             _transaction = _connection.BeginTransaction();
//         }

//         public IAccountRepository Account
//         {
//             get { return _accountRepository ??= new AccountRepository(_transaction, _mapper); }
//         }

//         public void Dispose()
//         {
//             Dispose(true);
//             GC.SuppressFinalize(this);
//         }

//         protected virtual void Dispose(bool disposing)
//         {
//             if (!_disposed)
//             {
//                 if (disposing)
//                 {
//                     _transaction.Dispose();
//                     _connection.Dispose();
//                 }
//                 _disposed = true;
//             }
//         }
//     }
// }
