using System.Security.Claims;
using AccountsBackend.BusinessLogic.Services.AuthService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AccountsBackend.WebAPI.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        // await HttpContext.SignOutAsync();
        Response.Cookies.Delete("cool-cookies");
        return NoContent();
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> LoginAsync(UserRequest request) 
    {
        var result = await authService.LoginUserAsync(request);

        if (result is null)
            return Unauthorized();
        
        Response.Cookies.Append("cool-cookies", result, new CookieOptions{
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(30)
        });
        
        return result;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(string surname, string firstName, string middleName, string login, string password) 
    {
        await authService.RegisterUserAsync(surname, firstName, middleName, login, password);
        return Ok();
    }  

    [HttpGet("profile")]
    public async Task<IActionResult?> GetUserByIdAsync ()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await authService.GetUserByIdAsync(Convert.ToInt32(userId));
        return Ok(result);
            
    }
}