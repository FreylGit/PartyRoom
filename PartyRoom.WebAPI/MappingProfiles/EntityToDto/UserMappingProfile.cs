using AutoMapper;
using PartyRoom.Contracts.DTOs.Room;
using PartyRoom.Contracts.DTOs.User;
using PartyRoom.Domain.Entities;


namespace PartyRoom.WebAPI.MappingProfiles.EntityToDto
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

            CreateMap<PublicUserDTO, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

            CreateMap<UserRegistrationDTO, ApplicationUser>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));
            CreateMap<ApplicationUser, UserDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirtsName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

            CreateMap<ApplicationUser, PublicUserDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

            CreateMap<ApplicationUser, UserRegistrationDTO>()
               .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
               .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));

            CreateMap<Room, RoomCreateDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type));
            CreateMap<RoomCreateDTO, Room>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type));

            CreateMap<Room, RoomDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId));
            CreateMap<RoomDto, Room>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId));
            CreateMap<RoomCreateDTO, Room>();

        }
    }
}
