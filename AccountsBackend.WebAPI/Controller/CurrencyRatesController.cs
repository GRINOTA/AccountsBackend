using AccountsBackend.BusinessLogic.Services.CurrencyRatesService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountsBackend.WebAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CurrencyRatesController(ICurrencyRatesService service) : ControllerBase
    {
        [HttpGet("{idTargetRate:int}")]
        public async Task<IActionResult> GetRateByIdTargeetRate([FromRoute]int idTargetRate)
        {
            var result = await service.GetCurrencyRateByIdTargerCurrency(idTargetRate);
            return Ok(result);
        }               
    }
}