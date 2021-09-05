using Scapho.Application.POCO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scapho.Application.FIProjection
{
    public class FIProjectionCalculator
    {
        private static decimal GetTargetNetWorth(decimal currentMonthlySpending, Percentage expectedYearlyWithdrawalRate)
        {
            return currentMonthlySpending * 12 / expectedYearlyWithdrawalRate;
        }
        public int GetExpectedMonthsForIndependence(FinancialIndepCalcInputs inputs)
        {
            var targetNetWorth = GetTargetNetWorth(inputs.CurrentMonthlySpending, inputs.ExpectedYearlyWithdrawalRateAfterIndependence);

            var iterationNetIncome = inputs.CurrentMonthlyNetIncome;
            var portfolioWorth = inputs.CurrentInvestmentPortfolioWorth;
            int months = 0;

            while (portfolioWorth < targetNetWorth)
            {
                months++;
                var addedToPortfolio = iterationNetIncome - inputs.CurrentMonthlySpending;
                if (IsTwelfthMonth(months))
                {
                    iterationNetIncome *= (1 + inputs.ExpectedYearlyIncomeGrowth);
                }
                portfolioWorth *= (1 + (inputs.ExpectedYearlyPortfolioGrowth / 12));
                portfolioWorth += addedToPortfolio;
            }

            return months;
        }

        private static bool IsTwelfthMonth(int months)
        {
            return months % 12 == 0;
        }
    }
}
