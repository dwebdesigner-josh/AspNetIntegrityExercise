using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Account;
using api.Enums;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class AccountService : IAccountService
    {

        private readonly IAccountRepository _accountRepo;

        public AccountService(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }
        
        public async Task<AccountBalanceResponseDTO> DepositAsync(AccountTransactionRequestDTO accountDepositDTO)
        {
            var account = await _accountRepo.GetByIdAsync(accountDepositDTO.AccountId);

            if(account == null || account.CustomerId != accountDepositDTO.CustomerId)
            {
                return new AccountBalanceResponseDTO
                {
                    Succeeded = false,
                    ErrorType = AccountErrorType.NotFound
                };
            }
            if (accountDepositDTO.Amount <= 0)
            {
                return new AccountBalanceResponseDTO
                {
                    Succeeded = false,
                    ErrorType = AccountErrorType.InvalidAmount
                };
                
            }

            account.Balance += accountDepositDTO.Amount;

            try
            {
                await _accountRepo.UpdateAsync(account);
            }
            catch (DbUpdateException)
            {
                return new AccountBalanceResponseDTO
                {
                    Succeeded = false,
                    ErrorType = AccountErrorType.ServerError
                };
                
            }

            return new AccountBalanceResponseDTO
            {
                CustomerId = accountDepositDTO.CustomerId,
                AccountId =  accountDepositDTO.AccountId,
                Balance = account.Balance,
                Succeeded = true,
                ErrorType = AccountErrorType.None
            };
        }

        public async Task<AccountBalanceResponseDTO> WithdrawAsync(AccountTransactionRequestDTO accountWithdrawDTO)
        {
            var account = await _accountRepo.GetByIdAsync(accountWithdrawDTO.AccountId);

            if(account == null || account.CustomerId != accountWithdrawDTO.CustomerId)
            {
                return new AccountBalanceResponseDTO
                {
                    Succeeded = false,
                    ErrorType = AccountErrorType.NotFound
                };
            }
            if (accountWithdrawDTO.Amount <= 0)
            {
                return new AccountBalanceResponseDTO
                {
                    Succeeded = false,
                    ErrorType = AccountErrorType.InvalidAmount
                };
            }
            if (account.Balance < accountWithdrawDTO.Amount) // cannot bring balance below 0
            {
                return new AccountBalanceResponseDTO
                {
                    Succeeded = false,
                    ErrorType = AccountErrorType.InsufficientFunds
                };
            }


            account.Balance -= accountWithdrawDTO.Amount;

            try
            {
                await _accountRepo.UpdateAsync(account);
            }
            catch (DbUpdateException)
            {
                return new AccountBalanceResponseDTO
                {
                    Succeeded = false,
                    ErrorType = AccountErrorType.ServerError
                };
            }

            return new AccountBalanceResponseDTO
            {
                CustomerId = accountWithdrawDTO.CustomerId,
                AccountId =  accountWithdrawDTO.AccountId,
                Balance = account.Balance,
                Succeeded = true,
                ErrorType = AccountErrorType.None
            };

        }
    }
}