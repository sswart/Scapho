using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Scapho.Application.Tests
{
    public class MonthlySavingsTests
    {
        private readonly MonthlySavings _sut;

        public MonthlySavingsTests()
        {
            _sut = new MonthlySavings();
        }
        [Fact]
        public async Task Amount_BiggerThanZero_IsSavedAsync()
        {
            int amount = 1;

            await _sut.SetAsync(amount);

            var amt = await _sut.GetAsync();
            amt.Should().Be(amount);
        }

        [Fact]
        public async Task Amount_Zero_IsSavedAsync()
        {
            await _sut.SetAsync(1);
            await _sut.SetAsync(0);

            var amt = await _sut.GetAsync();
            amt.Should().Be(0);
        }

        [Fact]
        public void Amount_Negative_ThrowsException()
        {
            _sut.Invoking(async t => await t.SetAsync(-1))
                .Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public async Task Amount_Negative_NotSavedAsync()
        {
            await _sut.SetAsync(1);
            try
            {
                await _sut.SetAsync(-1);
            }
            catch { }

            var amt = await _sut.GetAsync();
            amt.Should().Be(1);
        }
    }
}
