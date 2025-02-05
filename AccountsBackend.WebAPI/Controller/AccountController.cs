using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AccountsBackend.BusinesLogic;
using Microsoft.AspNetCore.Authorization;

namespace AccountsBackend.WebAPI;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [HttpGet("{idUser:int}")]
    public async Task<IActionResult> GetByUserIdAsync([FromRoute]int idUser)
    {
        var result = await accountService.GetByUserIdAsync(idUser);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(int idUser, int idCurrency, string number) 
    {
        
        await accountService.CreateAsync(idUser, idCurrency, number);  
        return NoContent();    
    }
}