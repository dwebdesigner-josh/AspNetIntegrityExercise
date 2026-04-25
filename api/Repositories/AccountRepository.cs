using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace api.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDBContext _context;

        public AccountRepository(ApplicationDBContext context) 
        {
            _context = context;
        }
        
        public async Task<Account?> GetByIdAsync(int id)
        {
            return await _context.Accounts.FindAsync(id); // Find is better than FirstOrDefault for Id's from Databases with EF - checks DbContext memory (caching) so can be faster
            // FindAsync returns null if not found
        }

        public async Task UpdateAsync(Account accountModel)
        {
            ArgumentNullException.ThrowIfNull(accountModel);

            _context.Accounts.Update(accountModel); // not necessary, but good for redundancy - possibly change to only update specific values manually, or use Attach() to avoid marking unchanged fields as modified
            await _context.SaveChangesAsync();
        }
    }
}