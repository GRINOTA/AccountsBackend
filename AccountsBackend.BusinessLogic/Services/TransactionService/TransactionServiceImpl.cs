using AccountsBackend.Data;
using AccountsBackend.Data.Models;

namespace AccountsBackend.BusinesLogic;

internal class TransactionServiceImpl: ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionServiceImpl(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }   

    public async Task CreateAsync(int idSenderAccount, int idRecipientAccount, decimal amount, CancellationToken cancellationToken = default)
    {
        Transaction transaction = new Transaction 
        {
            SenderAccountId = idSenderAccount,
            RecipientAccountId = idRecipientAccount,
            Amount = amount
        };           

        await _transactionRepository.CreateAsync(transaction, cancellationToken);
    }
}