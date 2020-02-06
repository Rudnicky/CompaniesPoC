using CompaniesPoC.Core.Interfaces;
using CompaniesPoC.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompaniesPoC.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<long> Add(T entity)
        {
            _context.Set<T>().Add(entity);

            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChangesAsync();
        }

        public async Task<T> Get(long Id)
        {
            return await _context.Set<T>().FindAsync(Id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
