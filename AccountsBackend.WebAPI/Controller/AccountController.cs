using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AccountsBackend.BusinessLogic.Services.AccountService;

namespace AccountsBackend.WebAPI.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [HttpGet("users/{idUser:int}")]
    public async Task<IActionResult> GetByUserIdAsync([FromRoute]int idUser)
    {
        var result = await accountService.GetByUserIdAsync(idUser);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(int idUser, int idCurrency) 
    {
        await accountService.CreateAsync(idUser, idCurrency);  
        return NoContent();    
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute]int id)
    {
        var result = await accountService.GetByIdAsync(id);
        return Ok(result);
    }
}