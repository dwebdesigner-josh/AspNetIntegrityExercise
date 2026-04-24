using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(int id); // Find/FirstOrDefault in Repository can return null - need nullable reference type

        Task UpdateAsync(Account accountModel);
        
    }
}