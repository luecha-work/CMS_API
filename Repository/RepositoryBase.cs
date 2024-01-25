// using System;
// using System.Collections.Generic;
// using System.Data;
// using System.Linq;
// using System.Reflection;
// using System.Text;
// using System.Threading.Tasks;
// using Dapper;
// using Npgsql;
// using NpgsqlTypes;
// using Repository.Contracts;
// using Shared.Query;

// namespace Repository
// {
//     internal abstract class RepositoryBase<T> : IRepositoryBase<T>
//         where T : class
//     {
//         protected IDbTransaction _transaction { get; private set; }
//         protected IDbConnection? _connection
//         {
//             get { return _transaction.Connection; }
//         }
//         private readonly string _tableName;

//         protected RepositoryBase(IDbTransaction transaction)
//         {
//             _transaction = transaction;
//             _tableName = ConvertToDbColumnName(typeof(T).Name);
//         }

//         public T GetById(int id)
//         {
//             var properties = GenerateListOfProperties()
//                 .Select(prop => $"{GetColumnName(prop.Name)} AS {prop.Name}");
//             var columns = string.Join(", ", properties);

//             var sql = $"SELECT {columns} FROM {_tableName} WHERE Id = @Id";
//             var result = _connection.QuerySingleOrDefault<T>(sql, new { Id = id });
//             return result;
//         }

//         public IEnumerable<T> GetAll()
//         {
//             var properties = GenerateListOfProperties()
//                 .Select(prop => $"{GetColumnName(prop.Name)} AS {prop.Name}");
//             var columns = string.Join(", ", properties);

//             var sql = $"SELECT {columns} FROM {_tableName}";
//             var result = _connection.Query<T>(sql);
//             return result;
//         }

//         public int Create(T entity)
//         {
//             var insertQuery = GenerateInsertQuery();

//             var result = _connection.Execute(insertQuery, entity, _transaction);
//             return result;
//         }

//         public int CreateWithOutputId(T entity)
//         {
//             var insertQuery = new StringBuilder(GenerateInsertQuery());

//             insertQuery.Append(" RETURNING id");

//             var result = _connection.ExecuteScalar<int>(
//                 insertQuery.ToString(),
//                 entity,
//                 _transaction
//             );
//             return result;
//         }

//         public int Update(T entity)
//         {
//             var updateQuery = GenerateUpdateQuery();

//             var result = _connection.Execute(updateQuery, entity, _transaction);
//             return result;
//         }

//         public int Delete(int id)
//         {
//             var sql = $"DELETE FROM {_tableName} WHERE Id = @Id";
//             var result = _connection.Execute(sql, new { Id = id }, _transaction);
//             return result;
//         }

//         // PRIVATE
//         private static string GetColumnName(string propertyName)
//         {
//             // add an underscore before each uppercase letter that is followed by a lowercase letter,
//             // then make the entire string uppercase
//             return new string(
//                 propertyName
//                     .SelectMany(
//                         (c, i) =>
//                             i > 0 && char.IsLower(propertyName[i - 1]) && char.IsUpper(c)
//                                 ? new[] { '_', c }
//                                 : new[] { c }
//                     )
//                     .ToArray()
//             ).ToUpper();
//         }

//         private static Func<PropertyInfo, bool> GetIsNotIdentityFunc()
//         {
//             return p => !p.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase);
//         }

//         private static List<PropertyInfo> GenerateListOfProperties(
//             Func<PropertyInfo, bool>? selector = null
//         )
//         {
//             var properties = typeof(T).GetProperties();

//             if (selector != null)
//                 properties = properties.Where(selector).ToArray();

//             var simpleTypes = new[]
//             {
//                 typeof(string),
//                 typeof(DateTime),
//                 typeof(DateTime?),
//                 typeof(int),
//                 typeof(int?),
//                 typeof(long),
//                 typeof(long?),
//                 typeof(decimal),
//                 typeof(decimal?),
//                 typeof(double),
//                 typeof(double?),
//                 // add more types if needed
//             };

//             return properties.Where(p => simpleTypes.Contains(p.PropertyType)).ToList();
//         }

//         private string GenerateInsertQuery()
//         {
//             var insertQuery = new StringBuilder($"INSERT INTO {_tableName} ");

//             insertQuery.Append('(');

//             var properties = GenerateListOfProperties(GetIsNotIdentityFunc());

//             properties.ForEach(prop =>
//             {
//                 insertQuery.Append($"{ConvertToDbColumnName(prop.Name)},");
//             });

//             insertQuery.Remove(insertQuery.Length - 1, 1).Append(") VALUES (");

//             properties.ForEach(prop =>
//             {
//                 insertQuery.Append($"@{prop.Name},");
//             });

//             insertQuery.Remove(insertQuery.Length - 1, 1).Append(')');

//             return insertQuery.ToString();
//         }

//         private string GenerateUpdateQuery()
//         {
//             var updateQuery = new StringBuilder($"UPDATE {_tableName} SET ");
//             var properties = GenerateListOfProperties(GetIsNotIdentityFunc());

//             properties.ForEach(prop =>
//             {
//                 updateQuery.Append($"{ConvertToDbColumnName(prop.Name)} = @{prop.Name},");
//             });

//             updateQuery.Remove(updateQuery.Length - 1, 1);
//             updateQuery.Append($" WHERE {ConvertToDbColumnName("Id")} = @Id");

//             return updateQuery.ToString();
//         }

//         private static string ConvertToDbColumnName(string propertyName)
//         {
//             var result = new StringBuilder();
//             var letters = propertyName.ToCharArray();

//             foreach (var letter in letters)
//             {
//                 if (char.IsUpper(letter) && result.Length > 0)
//                 {
//                     result.Append('_');
//                 }

//                 result.Append(letter);
//             }

//             return result.ToString().ToLower();
//         }
//     }
// }
