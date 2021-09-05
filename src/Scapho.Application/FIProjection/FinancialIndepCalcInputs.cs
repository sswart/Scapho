using Scapho.Application.POCO;
using System;

namespace Scapho.Application.FIProjection
{
    public record FinancialIndepCalcInputs(
        Percentage ExpectedYearlyPortfolioGrowth,
        Percentage ExpectedYearlyWithdrawalRateAfterIndependence,
        Percentage ExpectedYearlyIncomeGrowth)
    {
        public FinancialIndepCalcInputs(
            decimal CurrentMonthlyNetIncome,
            decimal CurrentMonthlySpending,
            decimal CurrentInvestmentPortfolioWorth,
            Percentage ExpectedYearlyPortfolioGrowth,
            Percentage ExpectedYearlyWithdrawalRateAfterIndependence,
            Percentage ExpectedYearlyIncomeGrowth) : this(ExpectedYearlyPortfolioGrowth, ExpectedYearlyWithdrawalRateAfterIndependence, ExpectedYearlyIncomeGrowth)
        {
            this.CurrentMonthlyNetIncome = CurrentMonthlyNetIncome;
            this.CurrentMonthlySpending = CurrentMonthlySpending;
            this.CurrentInvestmentPortfolioWorth = CurrentInvestmentPortfolioWorth;
        }
        readonly decimal currentMonthlyNetIncome;
        readonly decimal currentMonthlySpending;
        readonly decimal currentInvestmentPortfolioWorth;

        public decimal CurrentMonthlyNetIncome
        {
            get => currentMonthlyNetIncome;
            init => currentMonthlyNetIncome = BiggerThanZero(value, nameof(CurrentMonthlyNetIncome));
        }

        public decimal CurrentMonthlySpending
        {
            get => currentMonthlySpending;
            init => currentMonthlySpending = NotNegative(value, nameof(CurrentMonthlySpending));
        }

        public decimal CurrentInvestmentPortfolioWorth
        {
            get => currentInvestmentPortfolioWorth;
            init => currentInvestmentPortfolioWorth = NotNegative(value, nameof(CurrentInvestmentPortfolioWorth));
        }

        private decimal NotNegative(decimal val, string propertyName) => val >= 0 ? val : throw new ArgumentException(propertyName);
        private decimal BiggerThanZero(decimal val, string propertyName) => val > 0 ? val : throw new ArgumentException(propertyName);
    };
}
