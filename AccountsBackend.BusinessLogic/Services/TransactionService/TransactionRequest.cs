namespace AccountsBackend.BusinessLogic.Services.TransactionService
{
    public class TransactionRequest
    {
        public string numberSenderAccount { get; set; }
        public string numberRecipientAccount { get; set; }
        public decimal amount { get; set; }
    }
}