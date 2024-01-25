using System.Data;
using System.Linq.Expressions;

namespace Contracts
{
    public interface IRepositoryBase<T>
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        int Create(T entity);
        int CreateWithOutputId(T entity);
        int Update(T entity);
        int Delete(int id);
    }
}
