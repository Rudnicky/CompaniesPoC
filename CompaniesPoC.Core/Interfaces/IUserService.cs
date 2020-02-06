using CompaniesPoC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompaniesPoC.Core.Interfaces
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetAll();
    }
}
