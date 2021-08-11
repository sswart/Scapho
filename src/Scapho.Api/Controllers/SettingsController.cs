using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scapho.Api.DTO;
using Scapho.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scapho.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> IndexAsync([FromServices]MonthlySavings monthlySavings, [FromBody]SettingsDTO settings)
        {
            await monthlySavings.SetAsync(settings.MonthlySavingsAmount);
            return Ok();
        }
    }
}
