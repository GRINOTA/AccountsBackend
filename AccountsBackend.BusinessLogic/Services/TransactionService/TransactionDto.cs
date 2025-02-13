using AccountsBackend.BusinessLogic.Mappings;
using AccountsBackend.BusinessLogic.Services.AccountService;
using AccountsBackend.Data.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccountsBackend.BusinessLogic.Services.TransactionService
{
    public class TransactionDto: IMapWith<Transaction>
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public decimal SenderBalance { get; set; }
        public AccountTransactionDto? Sender { get; set; }
        public AccountTransactionDto? Recipient { get; set; }
        public string RecipientNumber { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Transaction, TransactionDto>()
                .ForMember(dto => dto.Date,
                    opt => opt.MapFrom(t => t.Date))
                .ForMember(dto => dto.Amount, 
                    opt => opt.MapFrom(t => t.Amount))
                .ForMember(dto => dto.SenderBalance,
                    opt => opt.MapFrom(t => t.SenderAccount.Balance))
                .ForMember(dto => dto.Sender,
                    opt => opt.MapFrom(t => t.SenderAccount))
                .ForMember(dto => dto.Recipient, 
                    opt => opt.MapFrom(t => t.RecipientAccount))
                .ForMember(dto => dto.RecipientNumber,
                    opt => opt.MapFrom(t => t.RecipientAccount.Number));
        }        
    }
}