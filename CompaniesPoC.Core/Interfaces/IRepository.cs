using CompaniesPoC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompaniesPoC.Core.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T> Get(long Id);
        Task<IEnumerable<T>> GetAll();
        Task<long> Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
