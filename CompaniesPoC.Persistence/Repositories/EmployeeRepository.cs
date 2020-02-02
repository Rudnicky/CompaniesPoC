using CompaniesPoC.Core.Interfaces;
using CompaniesPoC.Core.Models;

namespace CompaniesPoC.Persistence.Repositories
{
    public sealed class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
