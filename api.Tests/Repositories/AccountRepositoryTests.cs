using System;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using api.Repositories;
using api.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace api.Tests.Repositories
{
    public class AccountRepositoryTests : IDisposable
    {
        private readonly DbContextFixture _fixture;

        public AccountRepositoryTests()
        {
            _fixture = new DbContextFixture();
        }

        private AccountRepository CreateRepository()
        {
            return new AccountRepository(_fixture.Context);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsAccount()
        {
            // Arrange
            _fixture.SeedTestData();
            var repository = CreateRepository();

            // Act
            var result = await repository.GetByIdAsync(17);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(17, result.Id);
            Assert.Equal(5, result.CustomerId);
            Assert.Equal(2175.13m, result.Balance);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            _fixture.SeedTestData();
            var repository = new AccountRepository(_fixture.Context);

            // Act
            var result = await repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_WithValidAccount_UpdatesBalance()
        {
            // Arrange
            _fixture.SeedTestData();
            var repository = new AccountRepository(_fixture.Context);
            var account = await repository.GetByIdAsync(17);
            Assert.NotNull(account);

            // Act
            account.Balance += 112.00m;
            await repository.UpdateAsync(account);

            // Assert
            var updated = await repository.GetByIdAsync(17);
            Assert.NotNull(updated);
            Assert.Equal(2287.13m, updated.Balance);
        }

        [Fact]
        public async Task UpdateAsync_WithNullAccount_ThrowsArgumentNullException()
        {
            _fixture.SeedTestData();
            // Arrange
            var repository = new AccountRepository(_fixture.Context);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => repository.UpdateAsync(null!));
        }

        [Fact]
        public async Task UpdateAsync_MultipleUpdates_PersistsChanges()
        {
            // Arrange
            _fixture.SeedTestData();
            var repository = new AccountRepository(_fixture.Context);
            var account = await repository.GetByIdAsync(17);
            Assert.NotNull(account);

            // Act - First update
            account.Balance = 5000.00m;
            await repository.UpdateAsync(account);

            // Act - Second update
            account.Balance = 6500.00m;
            await repository.UpdateAsync(account);

            // Assert
            var final = await repository.GetByIdAsync(17);
            Assert.NotNull(final);
            Assert.Equal(6500.00m, final.Balance);
        }

        public void Dispose()
        {
            // Teardown: runs after every test
            // can put extra teardown logic here in the future
            _fixture.Dispose();
        }
    }
}
