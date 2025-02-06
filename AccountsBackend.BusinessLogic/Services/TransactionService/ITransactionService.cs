namespace AccountsBackend.BusinesLogic;

public interface ITransactionService
{
    Task CreateAsync(int idSenderAccount, int idRecipientAccount, decimal amount, CancellationToken cancellationToken = default);  
    Task<List<TransactionDto>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default); 
}