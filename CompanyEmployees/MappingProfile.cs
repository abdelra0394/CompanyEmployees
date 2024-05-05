﻿using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace CompanyEmployees
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(c => c.FullAddress,
                    options => options
                    .MapFrom(x => string.Join(' ', x.Address, x.Country))
                    );

            CreateMap<CompanyForCreationDto, Company>();

            CreateMap<Employee, EmployeeDto>();

            CreateMap<EmployeeForCreationDto, Employee>();

            CreateMap<EmployeeForUpdateDto, Employee>();

            CreateMap<CompanyForUpdateDto, Company>();
        }
    }
}
