using AutoMapper;

namespace CompaniesPoC.Core.Mappers
{
    public class MapperConfiguration : Profile
    {
        public CompanyMapper CompanyMapper { get; set; }
        public EmployeeMapper EmployeeMapper { get; set; }
    }
}
