using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AccountsBackend.BusinesLogic;
using Microsoft.AspNetCore.Authorization;

namespace AccountsBackend.WebAPI;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionController(ITransactionService transactionService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync(int idSenderAccount, int idRecipientAccount, decimal amount) 
    {
        await transactionService.CreateAsync(idSenderAccount, idRecipientAccount, amount);
        return NoContent();

    }  
}