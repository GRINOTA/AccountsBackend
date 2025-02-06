using AccountsBackend.BusinessLogic.Mappings;
using AccountsBackend.BusinessLogic.Services.AccountService;
using AccountsBackend.Data.Models;
using AutoMapper;

namespace AccountsBackend.BusinessLogic.Services.TransactionService
{
    public class TransactionDto: IMapWith<Transaction>
    {

        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public AboutAccountDto? Sender { get; set; }
        public AboutAccountDto? Recipient { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Transaction, TransactionDto>()
                .ForMember(dto => dto.Date, 
                    opt => opt.MapFrom(t => t.Date))
                .ForMember(c => c.Amount,
                    opt => opt.MapFrom(t => t.Amount))
                .ForMember(dto => dto.Sender,
                    opt => opt.MapFrom(t => t.SenderAccount))
                .ForMember(dto => dto.Recipient,
                    opt => opt.MapFrom(t => t.RecipientAccount));    
        }
    }
}


