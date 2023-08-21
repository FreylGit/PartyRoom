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
              .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
              .ForMember(dest => dest.StartDate, opt => opt.MapFrom(dest => dest.StartDate));

            CreateMap<RoomCreateDTO, Room>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
               .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
               .ForMember(dest => dest.FinishDate, opt => opt.MapFrom(src => src.FinishDate));

            CreateMap<Room, RoomDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.FinishDate, opt => opt.MapFrom(src => src.FinishDate))
                .ForMember(dest => dest.IsStarted, opt => opt.MapFrom(src => src.IsStarted));
            CreateMap<RoomDto, Room>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.FinishDate, opt => opt.MapFrom(src => src.FinishDate))
                .ForMember(dest => dest.IsStarted, opt => opt.MapFrom(src => src.IsStarted));

            CreateMap<Room, RoomInfoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.IsStarted, opt => opt.MapFrom(src => src.IsStarted))
                .ForMember(dest => dest.FinishDate, opt => opt.MapFrom(src => src.FinishDate))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate));
        }
    }
}
