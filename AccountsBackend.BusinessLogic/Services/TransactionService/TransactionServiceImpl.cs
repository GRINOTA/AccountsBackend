using AccountsBackend.Data;
using AccountsBackend.Data.DataContext;
using AccountsBackend.Data.Models;
using AccountsBackend.Data.Repositories.TransactionRepository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AccountsBackend.BusinessLogic.Services.TransactionService
{
    internal class TransactionServiceImpl: ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly AccountsContext _context;

        public TransactionServiceImpl(
            ITransactionRepository transactionRepository, 
            IMapper mapper, 
            AccountsContext context)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _context = context;
        }   

        public async Task CreateAsync(TransactionRequest transactionRequest, CancellationToken cancellationToken = default)
        {
            var senderAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Number == transactionRequest.numberSenderAccount);
            var recipientAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Number == transactionRequest.numberRecipientAccount);
            
            if(senderAccount != null && recipientAccount != null)
            {
                if(senderAccount.Id != recipientAccount.Id) {
                    Transaction transaction = new Transaction 
                    {
                        SenderAccountId = senderAccount.Id,
                        RecipientAccountId = recipientAccount.Id,
                        Amount = transactionRequest.amount
                    };           

                    await _transactionRepository.CreateAsync(transaction, cancellationToken);
                }
            }
        }

        public async Task<List<TransactionDto>> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var transaction = await _transactionRepository.GetByUserIdAsync(userId, cancellationToken);

            return _mapper.Map<List<TransactionDto>>(transaction);
        }
    }
}