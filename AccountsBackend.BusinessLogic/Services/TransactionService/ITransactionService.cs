namespace AccountsBackend.BusinesLogic;

public interface ITransactionService
{
    Task CreateAsync(int idSenderAccount, int idRecipientAccount, decimal amount, CancellationToken cancellationToken = default);    
}