using System;

namespace CompaniesPoC.Core.Models
{
    public class Employee : EntityBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public JobTitle JobTitle { get; set; }
    }

    public enum JobTitle
    {
        Administrator,
        Developer,
        Architect,
        Manager
    }
}
