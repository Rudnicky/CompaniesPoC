using System;

namespace CompaniesPoC.Core.Models.DTO
{
    public class EmployeeDTO : EntityBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string JobTitle { get; set; }
    }
}
