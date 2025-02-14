using AccountsBackend.BusinessLogic.Services.AccountService;
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

        public async Task<List<AccountMovementDto>> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var transactions = await _transactionRepository.GetByUserIdAsync(userId, cancellationToken);
            var transactionDto = _mapper.Map<List<TransactionDto>>(transactions);

            var accounts = new Dictionary<string, AccountMovementDto>();

            foreach(var transaction in transactionDto) 
            {
                if(transaction.Sender != null) 
                {
                    if(!accounts.ContainsKey(transaction.Sender.AccountNumber)) 
                    {
                        accounts[transaction.Sender.AccountNumber] = new AccountMovementDto
                        {
                            AccountNumber = transaction.Sender.AccountNumber,
                            IdCurrency = transaction.Sender.IdCurrency,
                            Currency = transaction.Sender.Currency,
                            Balance = transaction.Sender.Balance,
                            Movements = new List<MovementDto>()
                        };

                        // accounts[transaction.Sender.AccountNumber].Balance -= transaction.Amount;
                        
                    };

                    accounts[transaction.Sender.AccountNumber].Movements.Add(new MovementDto
                    {
                        Date = transaction.Date,
                        Amount = -transaction.Amount,
                        // Balance = transaction.SenderBalance,
                        RecipientAccountNumber = transaction.RecipientNumber
                    });
                }

                if(transaction.Recipient != null) 
                {
                    if(!accounts.ContainsKey(transaction.Recipient.AccountNumber)) 
                    {
                        accounts[transaction.Recipient.AccountNumber] = new AccountMovementDto
                        {
                            AccountNumber = transaction.Recipient.AccountNumber,
                            IdCurrency = transaction.Recipient.IdCurrency,
                            Currency = transaction.Recipient.Currency,
                            Balance = transaction.Recipient.Balance,
                            Movements = new List<MovementDto>()
                        };

                       
                    }

                    accounts[transaction.Recipient.AccountNumber].Movements.Add(new MovementDto
                    {
                        Date = transaction.Date,
                        Amount = transaction.Amount,
                        // Balance = transaction.SenderBalance,
                        RecipientAccountNumber = transaction.Sender.AccountNumber
                    });


                    // accounts[transaction.Recipient.AccountNumber].Balance += transaction.Amount;
                    
                }
            }

            return accounts.Values.ToList();
        }
    }
}