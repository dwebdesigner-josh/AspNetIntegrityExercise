using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    
    // could do bool for this, but may need to add more statuses later on
    public enum AccountStatus
    {
        Active = 1,
        Closed = 2
    }

    public class Account
    {
        public int Id { get; set; } //PK
        public AccountStatus Status {get; set;} = AccountStatus.Active;

        public decimal Balance { get; set; }

        public int AccountTypeId {get; set;} //FK - 1 to 1 relationship - do this as separate class rather than enum so account types can have other data added to them later on
        public int CustomerId { get; set; } //FK -  child (1 customer to many accounts)
        public Customer? Customer { get; set; }// Optional navigation property (may not be loaded)
    }
}