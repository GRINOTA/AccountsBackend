using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AccountsBackend.BusinessLogic.Services.AccountService;
using System.Security.Claims;

namespace AccountsBackend.WebAPI.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [HttpGet("users")]
    public async Task<IActionResult> GetByUserIdAsync()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await accountService.GetByUserIdAsync(Convert.ToInt32(userId));
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(int idCurrency) 
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        await accountService.CreateAsync(Convert.ToInt32(userId), idCurrency);  
        return NoContent();    
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute]int id)
    {
        var result = await accountService.GetByIdAsync(id);
        return Ok(result);
    }
}