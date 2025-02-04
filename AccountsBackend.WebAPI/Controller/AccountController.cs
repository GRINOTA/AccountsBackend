using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AccountsBackend.BusinesLogic;

namespace AccountsBackend.WebAPI;

[ApiController]
[Route("[controller]")]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [HttpGet("{idUser:int}")]
    public async Task<IActionResult> GetByUserIdAsync([FromRoute]int idUser)
    {
        var result = await accountService.GetByUserIdAsync(idUser);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(int idUser, int idCurrency, string number, decimal balance) 
    {
        
        await accountService.CreateAsync(idUser, idCurrency, number, balance);  
        return NoContent();    
    }
}