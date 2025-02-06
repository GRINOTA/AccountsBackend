using AccountsBackend.Data;
using AccountsBackend.Data.Models;
using AccountsBackend.Data.Repositories.TransactionRepository;
using AutoMapper;

namespace AccountsBackend.BusinessLogic.Services.TransactionService
{
    internal class TransactionServiceImpl: ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionServiceImpl(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
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

        public async Task<List<TransactionDto>> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var transaction = await _transactionRepository.GetByUserIdAsync(userId, cancellationToken);

            return _mapper.Map<List<TransactionDto>>(transaction);
        }
    }
}