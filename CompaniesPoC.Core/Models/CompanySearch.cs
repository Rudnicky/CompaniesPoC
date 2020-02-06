using System;

namespace CompaniesPoC.Core.Models
{
    public class CompanySearch
    {
        public string Keyword { get; set; }
        public DateTime? EmployeeDateOfBirthFrom { get; set; }
        public DateTime? EmployeeDateOfBirthTo { get; set; }
        public string[] EmployeeJobTitles { get; set; }
    }
}
