using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.DTOs.Account
{
    public class AccountDepositRequestDTO
    {

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int AccountId { get; set; }
        
        [Required] 
        [Range(0.01, double.MaxValue, ErrorMessage="Deposit amount must be greater than 0")] 
        public decimal Amount { get; set; }

    }
}