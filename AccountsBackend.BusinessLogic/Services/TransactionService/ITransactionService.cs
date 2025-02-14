using AccountsBackend.BusinessLogic.Services.AccountService;

namespace AccountsBackend.BusinessLogic.Services.TransactionService
{
    public interface ITransactionService
    {
        Task CreateAsync(TransactionRequest transactionRequest, CancellationToken cancellationToken = default);  
        Task<List<AccountMovementDto>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default); 
    }
}