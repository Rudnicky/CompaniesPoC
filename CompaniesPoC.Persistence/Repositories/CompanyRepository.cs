using CompaniesPoC.Core.Interfaces;
using CompaniesPoC.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompaniesPoC.Persistence.Repositories
{
    public sealed class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public new async Task<IEnumerable<Company>> GetAll()
        {
            return  await _context.Companies
                    .Include(b => b.Employees)
                    .ToListAsync();
        }

        public async Task<bool> Exists(long Id)
        {
            return await _context.Companies.FirstOrDefaultAsync(x => x.Id == Id) != null;
        }

        public async Task<Company> FindByName(string companyName)
        {
            return await _context.Companies.FirstOrDefaultAsync(x => x.Name == companyName);
        }
    }
}
