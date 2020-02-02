using System.Collections.Generic;

namespace CompaniesPoC.Core.Models.DTO
{
    public class CompanyDTO : EntityBase
    {
        public string Name { get; set; }
        public int EstablishmentYear { get; set; }
        public ICollection<EmployeeDTO> Employees { get; set; }
    }
}
