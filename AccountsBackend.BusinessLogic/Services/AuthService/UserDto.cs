using AccountsBackend.BusinesLogic.Mapping;
using AccountsBackend.Data.Models;
using AutoMapper;

namespace AccountsBackend.BusinesLogic;

public class UserDto : IMapWith<User>
{
    public string Login { get; set; }
    public string Surname { get; set; }
    public string FirstName { get; set; } 
    public string? MiddleName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserDto>()
            .ForMember(dto => dto.Surname, 
                opt => opt.MapFrom(u => u.Surname))
            .ForMember(dto => dto.FirstName,
                opt => opt.MapFrom(u => u.FirstName))
            .ForMember(dto => dto.MiddleName,
                opt => opt.MapFrom(u => u.MiddleName))
            .ForMember(dto => dto.Login,
                opt => opt.MapFrom(u => u.Login));
    }   
}