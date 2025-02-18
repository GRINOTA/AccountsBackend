using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountsBackend.BusinessLogic.Services.TransactionService
{
    public class MovementDto
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int IdCurrency { get; set; }
        // public decimal Balance { get; set; }
        public string RecipientAccountNumber { get; set; }
    }
}