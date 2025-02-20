using AccountsBackend.BusinessLogic.Services.CurrencyRatesService;
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
        private readonly ICurrencyRatesService _currencyRatesService;

        public TransactionServiceImpl(
            ITransactionRepository transactionRepository, 
            IMapper mapper, 
            AccountsContext context,
            ICurrencyRatesService currencyRatesService)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _context = context;
            _currencyRatesService = currencyRatesService;
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
            

            var transactions = await _transactionRepository
                .GetByUserIdAsync(userId, cancellationToken);

            var transactionDto = _mapper.Map<List<TransactionDto>>(transactions);

            var accounts = new Dictionary<string, AccountMovementDto>();

            foreach(var transaction in transactionDto) 
            {
                if(transaction.Sender != null && transaction.SenderUserId == userId) 
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
                    };

                    accounts[transaction.Sender.AccountNumber].Movements.Add(new MovementDto
                    {
                        Date = transaction.Date,
                        Amount = -transaction.Amount,
                        IdCurrency = transaction.Sender.IdCurrency,
                        RecipientAccountNumber = transaction.Recipient.AccountNumber,
                    });
                }

                if(transaction.Recipient != null && transaction.RecipientUserId == userId) 
                {
                    decimal amountRecipient = transaction.Amount;
                    
                    if(transaction.Sender.IdCurrency != transaction.Recipient.IdCurrency) 
                    {
                        var getCurrentCurrencyRate = await _currencyRatesService.GetCurrencyRateByIdTargerCurrency(transaction.Recipient.IdCurrency);
                        amountRecipient = getCurrentCurrencyRate.Rate * transaction.Amount;
                    }

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
                        Amount = amountRecipient,
                        IdCurrency = transaction.Recipient.IdCurrency,
                        RecipientAccountNumber = transaction.Sender.AccountNumber,
                    });            
                }
            }

            return accounts.Values.ToList();
        }
    }
}