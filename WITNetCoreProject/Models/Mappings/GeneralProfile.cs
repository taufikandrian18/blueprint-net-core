using System;
using AutoMapper;
using WITNetCoreProject.Models.Dtos;
using WITNetCoreProject.Models.Entities;

namespace WITNetCoreProject.Models.Mappings {

    public class GeneralProfile : Profile {

        public GeneralProfile() {

            CreateMap<Users, UserDto>();
            CreateMap<UserForCreationDto, UserDto>();
            CreateMap<UserForCreationDto, Users>();
            CreateMap<UserForUpdateDto, Users>();
            CreateMap<UserForDeletionDto, Users>();
        }
    }
}
