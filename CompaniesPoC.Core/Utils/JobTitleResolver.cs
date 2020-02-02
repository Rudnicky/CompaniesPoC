using AutoMapper;
using CompaniesPoC.Core.Models;
using CompaniesPoC.Core.Models.DTO;
using System;

namespace CompaniesPoC.Core.Utils
{
    public class JobTitleResolver : IValueResolver<EmployeeDTO, Employee, JobTitle>
    {
        public JobTitle Resolve(EmployeeDTO source, Employee destination, JobTitle destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.JobTitle))
            {
                Enum.TryParse(source.JobTitle, out JobTitle jobTitle);

                return jobTitle;
            }

            return JobTitle.Administrator;
        }
    }
}
