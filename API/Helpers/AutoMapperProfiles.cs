﻿using API.DTOs;
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
            
        CreateMap<Family, FamilyDto>();

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