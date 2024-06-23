using AutoMapper;
using Employee_Management_System.DTO;
using Employee_Management_System.Entities;

namespace Employee_Management_System.Common
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<EmployeeBasicEntity, EmployeeBasicDTO>().ReverseMap();
            CreateMap<EmployeeAdditionalDTO, EmployeeAdditionalEntity>().ReverseMap();
        }
    }
}
