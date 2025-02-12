namespace AccountsBackend.BusinessLogic.Services.TransactionService
{
    public interface ITransactionService
    {
        Task CreateAsync(TransactionRequest transactionRequest, CancellationToken cancellationToken = default);  
        Task<List<TransactionDto>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default); 
    }
}