using System;
using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Tests.Fixtures
{
    public class DbContextFixture : IDisposable
    {
        private readonly ApplicationDBContext _context;

        public ApplicationDBContext Context => _context;

        public DbContextFixture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
                .Options;

            _context = new ApplicationDBContext(options);

            _context.Database.EnsureCreated();

        }

        public void SeedTestData()
        {

            _context.ChangeTracker.Clear();  // avoid tracking data from previous tests

            var customer = new Customer
            {
                Id = 5,
                Name = "John Doe"
            };

            var account = new Account
            {
                Id = 17,
                CustomerId = 5,
                AccountTypeId = 1,
                Balance = 2175.13m,
                Status = AccountStatus.Active
            };

            _context.Customers.Add(customer);
            _context.Accounts.Add(account);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
