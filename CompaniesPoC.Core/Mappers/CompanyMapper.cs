using AutoMapper;
using CompaniesPoC.Core.Models;
using CompaniesPoC.Core.Models.DTO;

namespace CompaniesPoC.Core.Mappers
{
    public class CompanyMapper : Profile
    {
        public CompanyMapper()
        {
            CreateMap<Company, CompanyDTO>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(x => x.EstablishmentYear, opt => opt.MapFrom(src => src.EstablishmentYear))
                .ForMember(x => x.Employees, opt => opt.MapFrom(src => src.Employees));

            CreateMap<CompanyDTO, Company>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(x => x.EstablishmentYear, opt => opt.MapFrom(src => src.EstablishmentYear))
                .ForMember(x => x.Employees, opt => opt.MapFrom(src => src.Employees));
        }
    }
}
