using AccountsBackend.BusinesLogic.Mapping;
using AccountsBackend.Data.Models;
using AutoMapper;

namespace AccountsBackend.BusinesLogic;

public class AccountDto : IMapWith<Account>
{
    public string? Number { get; set; }
    public string? Currency { get; set; } 

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Account, AccountDto>()
            .ForMember(g => g.Number, 
                opt => opt.MapFrom(a => a.Number))
            .ForMember(g => g.Currency,
                opt => opt.MapFrom(a => a.Currency.Code));
    }   
}