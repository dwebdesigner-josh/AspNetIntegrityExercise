using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Customer
    {
        public int Id { get; set; } //PK
        public string Name { get; set; } = string.Empty;
        public List<Account> Accounts { get; set; } = new List<Account> ();  // 1 customer to many accounts

    }
}