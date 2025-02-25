using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AccountsBackend.BusinessLogic.Services.TransactionService;
using System.Security.Claims;
using Npgsql;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace AccountsBackend.WebAPI.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionController(ITransactionService transactionService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody]TransactionRequest transactionRequest) 
    {
        try 
        {
            await transactionService.CreateAsync(transactionRequest);
            return NoContent();
        }
        catch(DbUpdateException ex)
        {
    
            return StatusCode((int)HttpStatusCode.InternalServerError, new {message = ex.InnerException.Message});
            
            
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetByUserIdAsync()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await transactionService.GetByUserIdAsync(Convert.ToInt32(userId));
        return Ok(result);
    }
}