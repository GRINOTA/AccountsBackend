using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AccountsBackend.BusinesLogic;
using Microsoft.AspNetCore.Authorization;

namespace AccountsBackend.WebAPI;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CurrencyController(ICurrencyService currencyService) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute]int id)
    {
        var result = await currencyService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await currencyService.GetAllAsync();
        return Ok(result);
    }
}