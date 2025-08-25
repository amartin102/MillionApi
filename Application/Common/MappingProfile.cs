using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using AutoMapper;
using Domain.Entities;

namespace Application.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PropertyItemCreateDto, Property>();
            CreateMap<Property, PropertyItemDto>();
            //CreateMap<OwnerDto, Owner>();
            //CreateMap<PropertyImageDto, PropertyImage>();
            //CreateMap<PropertyTraceDto, PropertyTrace>();
        }
    }
}
