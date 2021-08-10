using System;
using System.Threading.Tasks;

namespace Scapho.Application
{
    public class MonthlySavings
    {
        private void SaveAmount(int amount)
        {
            Amount = amount;
        }

        public Task SetAsync(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Should be bigger than 0");
            }
            SaveAmount(amount);
            return Task.CompletedTask;
        }

        public Task<int> GetAsync() => Task.FromResult(Amount);

        private int Amount { get; set; }
    }
}
