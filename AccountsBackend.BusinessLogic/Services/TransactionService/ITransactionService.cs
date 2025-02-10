namespace AccountsBackend.BusinessLogic.Services.TransactionService
{
    public interface ITransactionService
    {
        Task CreateAsync(string numberSenderAccount, string numberRecipientAccount, decimal amount, CancellationToken cancellationToken = default);  
        Task<List<TransactionDto>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default); 
    }
}