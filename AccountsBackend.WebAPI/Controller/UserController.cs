using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AccountsBackend.BusinesLogic;

namespace AccountsBackend.WebAPI;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("auth")]
    public async Task<ActionResult<string>> AuthAsync(UserRequest request) 
    {
        var result = await userService.GetUserByLoginAsync(request);

        if (result is null)
            return Unauthorized();
        
        Response.Cookies.Append("cool-cookies", result);
        return result;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(string surname, string firstName, string middleName, string login, string password) 
    {
        await userService.CreateUserAsync(surname, firstName, middleName, login, password);
        return Ok();
    }  
}