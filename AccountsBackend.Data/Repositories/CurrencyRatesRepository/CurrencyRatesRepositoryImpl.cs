using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountsBackend.Data.DataContext;
using AccountsBackend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountsBackend.Data.Repositories.CurrencyRatesRepository
{
    internal class CurrencyRatesRepositoryImpl(AccountsContext context) : ICurrencyRatesRepository
    {
        public async Task<CurrencyRate> GetCurrencyRateByIdTargerRate(int idTargetCurrency)
        {
            var currencyRate = await context.CurrencyRates
                .Include(r => r.BaseCurrency)
                .Include(r => r.TargetCurrency)
                .Where(r => r.TargetCurrencyId == idTargetCurrency)
                .OrderByDescending(r => r.Date)
                .FirstOrDefaultAsync();

            return currencyRate;
        }
    }
}