using AutoMapper;
using ParksAPI.Models;
using ParksAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParksAPI.ParkMapper
{
    public class ParkMappings : Profile
    {
        public ParkMappings()
        {
            CreateMap<NationalPark, NationalParkDTO>().ReverseMap();
            CreateMap<Trail, TrailDTO>().ReverseMap();
            CreateMap<Trail, TrailCreatelDTO>().ReverseMap();
            CreateMap<Trail, TrailUpdatelDTO>().ReverseMap();
        }
    }
}
