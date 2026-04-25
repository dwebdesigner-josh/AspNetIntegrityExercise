using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Account;

namespace api.Interfaces
{
    public interface IAccountService
    {
        Task<AccountBalanceResponseDTO> DepositAsync(AccountTransactionRequestDTO accountDepositDTO);
        Task<AccountBalanceResponseDTO> WithdrawAsync(AccountTransactionRequestDTO accountWithdrawDTO);
    }
}