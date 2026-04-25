using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Account;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepo;
        public AccountController(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        // make a deposit
// The endpoint will receive the following JSON:
// {
// customerId: 5,
// accountId: 17,
// amount: 112.00
// }
// And should return:
// {
// customerId: 5,
// accountId: 17,
// balance: 2287.13,
// succeeded: true
// }

        [HttpPost("deposit")] // POST not PUT because this isn't a simple update
        public async Task<IActionResult> Deposit([FromBody] AccountTransactionRequestDTO accountDepositDTO)
        {
            var account = await _accountRepo.GetByIdAsync(accountDepositDTO.AccountId);

            if(account == null || account.CustomerId != accountDepositDTO.CustomerId)
            {
                return NotFound("Account not found for customer");
            }
            if (accountDepositDTO.Amount <= 0)
            {
                return BadRequest("Transaction amount must be greater than zero");
            }

            account.Balance += accountDepositDTO.Amount;

            try
            {
                await _accountRepo.UpdateAsync(account);
            }
            catch (DbUpdateException)
            {
                return BadRequest("Failed to process deposit");
            }

            return Ok(new AccountBalanceResponseDTO
            {
                CustomerId = accountDepositDTO.CustomerId,
                AccountId =  accountDepositDTO.AccountId,
                Balance = account.Balance,
                Succeeded = true
            });
        }
    }
}