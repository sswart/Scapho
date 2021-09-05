using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Scapho.Application.Tests
{
    public class Class1
    {
        [Fact]
        public void Test1()
        {
            var inputs = new FinancialIndepCalcInputs(
               3200,
               1500,
               35000,
               0.05m,
               0.04m,
               0.1m);

            var months = GetExpectedMonthsForIndependence(inputs);
            months.Should().Be(103);
        }

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

        public struct Percentage
        {
            public Percentage(decimal percAsDecimal)
            {
                Value = percAsDecimal;
            }
            public Percentage(int percAsNumber)
            {
                Value = percAsNumber / 100m;
            }

            public static implicit operator decimal(Percentage p) => p.Value;
            public static implicit operator Percentage(decimal p) => new(p);

            public decimal Value { get; init; }
        }

        private static decimal GetTargetNetWorth(decimal currentMonthlySpending, Percentage expectedYearlyWithdrawalRate)
        {
            return currentMonthlySpending * 12 / expectedYearlyWithdrawalRate;
        }
        private static int GetExpectedMonthsForIndependence(FinancialIndepCalcInputs inputs)
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
