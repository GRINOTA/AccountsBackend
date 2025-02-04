using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AccountsBackend.BusinesLogic;

namespace AccountsBackend.WebAPI;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("auth")]
    public async Task<IActionResult> AuthAsync(string login, string password) 
    {
        
        return Ok(new {User = await userService.GetUserByLoginAsync(login, password)});
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(string surname, string firstName, string middleName, string login, string password) 
    {
        await userService.CreateUserAsync(surname, firstName, middleName, login, password);
        return Ok();
    }  
}