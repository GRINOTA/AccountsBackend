using AccountsBackend.BusinessLogic.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace AccountsBackend.WebAPI.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<string>> LoginAsync(UserRequest request) 
    {
        var result = await authService.LoginUserAsync(request);

        if (result is null)
            return Unauthorized();
        
        Response.Cookies.Append("cool-cookies", result);
        return result;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(string surname, string firstName, string middleName, string login, string password) 
    {
        await authService.RegisterUserAsync(surname, firstName, middleName, login, password);
        return Ok();
    }  

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserByIdAsync ([FromRoute] int id)
    {
        var result = await authService.GetUserByIdAsync(id);
        return Ok(result);
    }
}