using AccountsBackend.BusinessLogic.Mappings;
using AccountsBackend.Data.Models;
using AutoMapper;

namespace AccountsBackend.BusinessLogic.Services.CurrencyService
{
    public class CurrencyDto: IMapWith<Currency>
    {
        public string NameCurrency { get; set; }
        public string CodeCurrency { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Currency, CurrencyDto>()
                .ForMember(c => c.NameCurrency, 
                    opt => opt.MapFrom(o => o.Name))
                .ForMember(c => c.CodeCurrency,
                    opt => opt.MapFrom(o => o.Code));
        }     
    }
}

