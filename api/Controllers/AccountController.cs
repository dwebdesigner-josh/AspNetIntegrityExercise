using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Account;
using api.Enums;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        private IActionResult HandleResult(AccountBalanceResponseDTO result)
    {
        if (result.Succeeded)
        {
            return Ok(new AccountBalanceResponseDTO
            {
                CustomerId = result.CustomerId,
                AccountId = result.AccountId,
                Balance = result.Balance,
                Succeeded = true
            });
        }
            

        switch (result.ErrorType)
        {
            case AccountErrorType.NotFound:
                return NotFound("Account not found for customer");

            case AccountErrorType.InvalidAmount:
                return BadRequest("Transaction amount must be greater than zero");

            case AccountErrorType.InsufficientFunds:
                return Conflict("Insufficient funds for this withdrawal");

            case AccountErrorType.ServerError:
                return StatusCode(500, "Failed to process transaction");

            default:
                return StatusCode(500, "Unknown error");
        }
    }


        [HttpPost("deposit")] // POST not PUT because this isn't a simple update
        public async Task<IActionResult> Deposit([FromBody] AccountTransactionRequestDTO accountDepositDTO)
        {
            var result = await _accountService.DepositAsync(accountDepositDTO);
            return HandleResult(result);
            
        }

        [HttpPost("withdraw")] // POST not PUT because this isn't a simple update
        public async Task<IActionResult> Withdraw([FromBody] AccountTransactionRequestDTO accountWithdrawDTO)
        {

            var result = await _accountService.WithdrawAsync(accountWithdrawDTO);
            return HandleResult(result);

        }
    }
}