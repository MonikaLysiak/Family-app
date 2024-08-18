using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.UserPhotos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

        CreateMap<AppUserFamily, MemberDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.User.Created))
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.User.UserPhotos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.User.Surname))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.User.Gender))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.User.DateOfBirth.CalculateAge()));

        CreateMap<Family, FamilyDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.FamilyPhotos.FirstOrDefault(x => x.IsMain).Url));

        CreateMap<FamilyList, FamilyListDto>();

        CreateMap<UserPhoto, PhotoDto>();
        CreateMap<FamilyPhoto, PhotoDto>();
        //CreateMap<FamilyMemberUpdateDto, AppUser>();
        CreateMap<RegisterDto, AppUser>();
        CreateMap<Message, MessageDto>()
            .ForMember(d => d.SenderPhotoUrl, o => o.MapFrom(s => s.Sender.UserPhotos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(d => d.FamilyPhotoUrl, o => o.MapFrom(s => s.Family.FamilyPhotos.FirstOrDefault(x => x.IsMain).Url));

        CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc): null);
    }
}