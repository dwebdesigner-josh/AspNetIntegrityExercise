using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.DTOs.Account
{
    public class AccountTransactionRequestDTO
    {

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int AccountId { get; set; }
        
        [Required] 
        [Range(0.01, double.MaxValue, ErrorMessage="Transaction amount must be greater than zero")] 
        public decimal Amount { get; set; }

    }
}