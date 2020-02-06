using CompaniesPoC.Core.Models;
using CompaniesPoC.Core.Models.DTO;
using CompaniesPoC.Core.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompaniesPoC.Core.Interfaces
{
    public interface ICompanyManager
    {
        Task<CustomResults<IEnumerable<CompanyDTO>>> GetAll();
        Task<CustomResults<string>> Add(CompanyDTO company);
        Task<CustomResults<string>> Update(CompanyDTO company, long id);
        Task<CustomResults<IEnumerable<CompanyDTO>>> Search(CompanySearch search);
        Task<CustomResults<string>> Delete(long id);
    }
}
