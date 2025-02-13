using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountsBackend.BusinessLogic.Services.TransactionService
{
    public class AccountMovementDto
    {
        public string AccountNumber { get; set; }      
        public int IdCurrency { get; set; } 
        public string Currency { get; set; }
        public decimal Balance { get; set; }
        public List<MovementDto> Movements { get; set; }
    }
}