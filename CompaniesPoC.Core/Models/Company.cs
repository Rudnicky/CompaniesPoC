using System.Collections.Generic;

namespace CompaniesPoC.Core.Models
{
    public class Company : EntityBase
    {
        public string Name { get; set; }
        public int EstablishmentYear { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
