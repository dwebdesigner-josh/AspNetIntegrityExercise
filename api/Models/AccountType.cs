using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class AccountType
    {
        public int Id { get; set; }
        public string Name {get; set;} = string.Empty;
        
        // for possible future implementation
        // public decimal MonthlyFee { get; set; }
        // public decimal Interest { get; set; }
    }
}