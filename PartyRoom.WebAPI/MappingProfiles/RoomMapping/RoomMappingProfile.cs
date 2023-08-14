using AutoMapper;
using PartyRoom.Contracts.DTOs.Room;
using PartyRoom.Domain.Entities;

namespace PartyRoom.WebAPI.MappingProfiles.RoomMapping
{
    public class RoomMappingProfile : Profile
    {
        public RoomMappingProfile()
        {
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
