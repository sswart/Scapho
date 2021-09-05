using FluentAssertions;
using Scapho.Application.FIProjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Scapho.Application.Tests
{
    public class FIProjectionCalculatorTests
    {
        [Fact]
        public void Simple_Projection_Should_BeCorrect()
        {
            var inputs = new FinancialIndepCalcInputs(
               2000,
               1000,
               0,
               0.05m,
               0.04m,
               0m);

            var months = new FIProjectionCalculator().GetExpectedMonthsForIndependence(inputs);
            months.Should().Be(196);
        }
    }
}
