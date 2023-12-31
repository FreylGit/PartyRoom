﻿using AutoMapper;
using PartyRoom.Contracts.DTOs.User;
using PartyRoom.Domain.Entities;

namespace PartyRoom.WebAPI.MappingProfiles.UserMapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserDTO, ApplicationUser>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                  .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirtsName))
                  .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                  .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                  .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                  .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));
            CreateMap<ApplicationUser, UserDTO>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                  .ForMember(dest => dest.FirtsName, opt => opt.MapFrom(src => src.FirstName))
                  .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                  .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                  .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                  .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

            CreateMap<UserPublicDTO, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));
            CreateMap<ApplicationUser, UserPublicDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

            CreateMap<UserRegistrationDTO, ApplicationUser>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));


            CreateMap<ApplicationUser, UserRegistrationDTO>()
               .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
               .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));

            CreateMap<ApplicationUser, UserProfileDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                  .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                  .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                  .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                  .ForMember(dest => dest.About, opt => opt.MapFrom(src => src.UserDetails.About))
                  .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UserDetails.ImagePath));
        }
    }
}
