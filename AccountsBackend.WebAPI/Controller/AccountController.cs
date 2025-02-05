using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AccountsBackend.BusinesLogic;
using Microsoft.AspNetCore.Authorization;
using AccountsBackend.Data.Models;

namespace AccountsBackend.WebAPI;

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