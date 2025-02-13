using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountsBackend.BusinessLogic.Mappings;
using AccountsBackend.Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace AccountsBackend.BusinessLogic.Services.CurrencyRatesService
{
    public class CurrencyRateDto: IMapWith<CurrencyRate>
    {
        public string BaseCurrency { get; set; }
        public string TargetCurrency { get; set; }
        public decimal Rate { get; set; }

        public void Mapping(Profile profile) {
            profile.CreateMap<CurrencyRate, CurrencyRateDto>()
                .ForMember(dto => dto.BaseCurrency,
                    opt => opt.MapFrom(r => r.BaseCurrency.Code))
                .ForMember(dto => dto.TargetCurrency,
                    opt => opt.MapFrom(r => r.TargetCurrency.Code))
                .ForMember(dto => dto.Rate,
                    opt => opt.MapFrom(r => r.Rate));
        }
    }
}