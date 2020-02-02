using CompaniesPoC.Core.Models;
using System.Threading.Tasks;

namespace CompaniesPoC.Core.Interfaces
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<bool> Exists(long Id);
        Task<Company> FindByName(string companyName);
    }
}
