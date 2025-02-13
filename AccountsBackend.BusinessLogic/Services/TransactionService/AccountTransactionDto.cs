using AccountsBackend.BusinessLogic.Mappings;
using AccountsBackend.BusinessLogic.Services.AccountService;
using AccountsBackend.Data.Models;
using AutoMapper;

namespace AccountsBackend.BusinessLogic.Services.TransactionService
{
    public class AccountTransactionDto: IMapWith<Account>
    {

        public string Currency { get; set; }
        public int IdCurrency { get; set; }
        public decimal Balance { get; set; }
        public string AccountNumber { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Account, AccountTransactionDto>()
                .ForMember(dto => dto.Currency, 
                    opt => opt.MapFrom(t => t.Currency.Code))
                .ForMember(dto => dto.IdCurrency,
                    opt => opt.MapFrom(t => t.Currency.Id))
                .ForMember(c => c.Balance,
                    opt => opt.MapFrom(t => t.Balance))
                .ForMember(dto => dto.AccountNumber,
                    opt => opt.MapFrom(t => t.Number));   
        }
    }
}


