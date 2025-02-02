using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AccountsBackend.BusinesLogic;

namespace AccountsBackend.WebAPI;

[ApiController]
[Route("Account")]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync(int idUser, int idCurrency, string number, decimal balance) 
    {
        
        await accountService.CreateAsync(idUser, idCurrency, number, balance);  
        return NoContent();    
    }
}