using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Scapho.Api.Controllers;
using Scapho.Application;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Scapho.Api.Tests
{
    public class SettingsControllerTests
    {
        private readonly SettingsController _controller = new SettingsController();

        [Fact]
        public async Task Settings_Saves_MonthlySavings()
        {
            var monthlySavings = new MonthlySavings();
            await _controller.IndexAsync(monthlySavings, new DTO.SettingsDTO(5));

            (await monthlySavings.GetAsync()).Should().Be(5);
        }

        [Fact]
        public async Task Settings_Returns_200()
        {
            var monthlySavings = new MonthlySavings();
            var result = await _controller.IndexAsync(monthlySavings, new DTO.SettingsDTO(5));
            result.Should().BeOfType<OkResult>();
        }
    }
}
