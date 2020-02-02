using AutoMapper;
using CompaniesPoC.Core.Models;
using CompaniesPoC.Core.Models.DTO;
using CompaniesPoC.Core.Utils;
using System;

namespace CompaniesPoC.Core.Mappers
{
    public class EmployeeMapper : Profile
    {
        public EmployeeMapper()
        {
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(x => x.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(x => x.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(x => x.JobTitle, opt => opt.MapFrom(src => src.JobTitle));

            CreateMap<EmployeeDTO, Employee>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(x => x.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(x => x.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(x => x.JobTitle, opt => opt.MapFrom(new JobTitleResolver()));

            CreateMap<JobTitle, string>().ConvertUsing(c => c.ToString());
        }
    }
}
