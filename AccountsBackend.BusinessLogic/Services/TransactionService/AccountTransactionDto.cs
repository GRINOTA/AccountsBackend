using AccountsBackend.BusinessLogic.Mappings;
using AccountsBackend.Data.Models;
using AutoMapper;

namespace AccountsBackend.BusinessLogic.Services.TransactionService
{
    public class AccountTransactionDto: IMapWith<Account>
    {
        public int UserId { get; set; }
        public string? Number { get; set; }
        public string? Currency { get; set; } 

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Account, AccountTransactionDto>()
                .ForMember(dto => dto.UserId,
                    opt => opt.MapFrom(a => a.UserId))
                .ForMember(dto => dto.Number, 
                    opt => opt.MapFrom(a => a.Number))
                .ForMember(dto => dto.Currency,
                    opt => opt.MapFrom(a => a.Currency.Code));
        }        
    }
}