using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using api.Enums;

namespace api.DTOs.Account
{
    public class AccountBalanceResponseDTO
    {
        // Response - no data validation needed
        public int? CustomerId { get; set; }
        public int? AccountId { get; set; }
        public decimal? Balance { get; set; }
        public bool Succeeded { get; set; }
        
        [JsonIgnore]
        public AccountErrorType ErrorType { get; set; }
        
    }
}