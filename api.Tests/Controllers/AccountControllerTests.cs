using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Controllers;
using api.Data;
using api.DTOs.Account;
using api.Interfaces;
using api.Models;
using api.Repositories;
using api.Services;
using api.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace api.Tests.Controllers
{
    public class AccountControllerTests : IDisposable
    {
        private readonly DbContextFixture _fixture;

        public AccountControllerTests() // setup - runs before every test
        {
            _fixture = new DbContextFixture();
            // _fixture.SeedTestData(); // Seeding is left to individual tests to avoid implicit shared setup

        }


        private AccountController CreateController()
        {
            var repository = new AccountRepository(_fixture.Context);
            var service = new AccountService(repository);

            return new AccountController(service);
        }


// -------------------------------------------------------------------------------------------
        // DEPOSIT TESTS
// -------------------------------------------------------------------------------------------


        [Fact]
        public async Task Deposit_WithValidRequest_ReturnsOkAndUpdatesBalance()
        {
            // Arrange
            _fixture.SeedTestData(); 
            var controller = CreateController();
            var depositRequest = new AccountTransactionRequestDTO
            {
                CustomerId = 5,
                AccountId = 17,
                Amount = 112.00m
            };

            // Act
            var result = await controller.Deposit(depositRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<AccountBalanceResponseDTO>(okResult.Value);
            
            Assert.Equal(5, response.CustomerId);
            Assert.Equal(17, response.AccountId);
            Assert.Equal(2287.13m, response.Balance);
            Assert.True(response.Succeeded);
        }

        [Fact]
        public async Task Deposit_WithNonexistentAccount_ReturnsNotFound()
        {
            // Arrange
            _fixture.SeedTestData(); 
            var controller = CreateController();
            var depositRequest = new AccountTransactionRequestDTO
            {
                CustomerId = 5,
                AccountId = 999,
                Amount = 100.00m
            };

            // Act
            var result = await controller.Deposit(depositRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Account not found for customer", notFoundResult.Value);
        }

        [Fact]
        public async Task Deposit_WithWrongCustomerId_ReturnsNotFound()
        {
            // Arrange
            _fixture.SeedTestData(); 
            var controller = CreateController();
            var depositRequest = new AccountTransactionRequestDTO
            {
                CustomerId = 999, // Wrong customer
                AccountId = 17,
                Amount = 100.00m
            };

            // Act
            var result = await controller.Deposit(depositRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Account not found for customer", notFoundResult.Value);
        }

        [Fact]
        public async Task Deposit_WithNegativeAmount_ReturnsBadRequest()
        {
            // Arrange
            _fixture.SeedTestData(); 
            var controller = CreateController();
            var depositRequest = new AccountTransactionRequestDTO
            {
                CustomerId = 5,
                AccountId = 17,
                Amount = -50.00m
            };

            // Act
            var result = await controller.Deposit(depositRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Transaction amount must be greater than zero", badRequestResult.Value);
        }

        [Fact]
        public async Task Deposit_WithZeroAmount_ReturnsBadRequest()
        {
            // Arrange
            _fixture.SeedTestData(); 
            var controller = CreateController();
            var depositRequest = new AccountTransactionRequestDTO
            {
                CustomerId = 5,
                AccountId = 17,
                Amount = 0m
            };

            // Act
            var result = await controller.Deposit(depositRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Transaction amount must be greater than zero", badRequestResult.Value);
        }

        [Fact]
        public async Task Deposit_WithSmallAmount_ReturnsOk()
        {
            // Arrange
            _fixture.SeedTestData(); 
            var controller = CreateController();
            var depositRequest = new AccountTransactionRequestDTO
            {
                CustomerId = 5,
                AccountId = 17,
                Amount = 0.01m
            };

            // Act
            var result = await controller.Deposit(depositRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<AccountBalanceResponseDTO>(okResult.Value);
            
            Assert.Equal(2175.14m, response.Balance);
            Assert.True(response.Succeeded);
        }

        [Fact]
        public async Task Deposit_WithLargeAmount_ReturnsOk()
        {
            // Arrange
            _fixture.SeedTestData(); 
            var controller = CreateController();
            var depositRequest = new AccountTransactionRequestDTO
            {
                CustomerId = 5,
                AccountId = 17,
                Amount = 10000.00m
            };

            // Act
            var result = await controller.Deposit(depositRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<AccountBalanceResponseDTO>(okResult.Value);
            
            Assert.Equal(12175.13m, response.Balance);
            Assert.True(response.Succeeded);
        }

        [Fact]
        public async Task Deposit_MultipleDeposits_BalanceAccumulates()
        {
            // Arrange
            _fixture.SeedTestData(); 
            var controller = CreateController();

            // Act - First deposit
            var result1 = await controller.Deposit(new AccountTransactionRequestDTO
            {
                CustomerId = 5,
                AccountId = 17,
                Amount = 100m
            });
            var response1 = Assert.IsType<OkObjectResult>(result1).Value as AccountBalanceResponseDTO;
            Assert.NotNull(response1);
            Assert.Equal(2275.13m, response1.Balance);

            // Act - Second deposit
            var result2 = await controller.Deposit(new AccountTransactionRequestDTO
            {
                CustomerId = 5,
                AccountId = 17,
                Amount = 200m
            });
            var response2 = Assert.IsType<OkObjectResult>(result2).Value as AccountBalanceResponseDTO;

            // Assert
            Assert.NotNull(response2);
            Assert.Equal(2475.13m, response2.Balance);
        }

// -------------------------------------------------------------------------------------------
        // WITHDRAW TESTS
// -------------------------------------------------------------------------------------------

        [Fact]
        public async Task Withdraw_WithValidRequest_ReturnsOkAndUpdatesBalance()
        {
            // Arrange
            _fixture.SeedTestData(); 
            var controller = CreateController();
            var withdrawRequest = new AccountTransactionRequestDTO
            {
                CustomerId = 5,
                AccountId = 17,
                Amount = 112.00m
            };

            // Act
            var result = await controller.Withdraw(withdrawRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<AccountBalanceResponseDTO>(okResult.Value);
            
            Assert.Equal(5, response.CustomerId);
            Assert.Equal(17, response.AccountId);
            Assert.Equal(2063.13m, response.Balance);
            Assert.True(response.Succeeded);
        }

        [Fact]
        public async Task Withdraw_WithNonexistentAccount_ReturnsNotFound()
        {
            // Arrange
            _fixture.SeedTestData(); 
            var controller = CreateController();
            var withdrawRequest = new AccountTransactionRequestDTO
            {
                CustomerId = 5,
                AccountId = 999,
                Amount = 100.00m
            };

            // Act
            var result = await controller.Withdraw(withdrawRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Account not found for customer", notFoundResult.Value);
        }

        [Fact]
        public async Task Withdraw_WithWrongCustomerId_ReturnsNotFound()
        {
            // Arrange
            _fixture.SeedTestData(); 
            var controller = CreateController();
            var withdrawRequest = new AccountTransactionRequestDTO
            {
                CustomerId = 999, // Wrong customer
                AccountId = 17,
                Amount = 100.00m
            };

            // Act
            var result = await controller.Withdraw(withdrawRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Account not found for customer", notFoundResult.Value);
        }

        [Fact]
        public async Task Withdraw_WithNegativeAmount_ReturnsBadRequest()
        {
            // Arrange
            _fixture.SeedTestData(); 
            var controller = CreateController();
            var withdrawRequest = new AccountTransactionRequestDTO
            {
                CustomerId = 5,
                AccountId = 17,
                Amount = -50.00m
            };

            // Act
            var result = await controller.Withdraw(withdrawRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Transaction amount must be greater than zero", badRequestResult.Value);
        }

        [Fact]
        public async Task Withdraw_WithZeroAmount_ReturnsBadRequest()
        {
            // Arrange
            _fixture.SeedTestData(); 
            var controller = CreateController();
            var withdrawRequest = new AccountTransactionRequestDTO
            {
                CustomerId = 5,
                AccountId = 17,
                Amount = 0m
            };

            // Act
            var result = await controller.Withdraw(withdrawRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Transaction amount must be greater than zero", badRequestResult.Value);
        }

        [Fact]
        public async Task Withdraw_WithSmallAmount_ReturnsOk()
        {
            // Arrange
            _fixture.SeedTestData(); 
            var controller = CreateController();
            var withdrawRequest = new AccountTransactionRequestDTO
            {
                CustomerId = 5,
                AccountId = 17,
                Amount = 0.01m
            };

            // Act
            var result = await controller.Withdraw(withdrawRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<AccountBalanceResponseDTO>(okResult.Value);
            
            Assert.Equal(2175.12m, response.Balance);
            Assert.True(response.Succeeded);
        }

        [Fact]
        public async Task Withdraw_WithLargeAmount_ReturnsOk()
        {
            // Arrange
            _fixture.SeedTestData(); 
            var controller = CreateController();
            var withdrawRequest = new AccountTransactionRequestDTO
            {
                CustomerId = 5,
                AccountId = 17,
                Amount = 2000.00m
            };

            // Act
            var result = await controller.Withdraw(withdrawRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<AccountBalanceResponseDTO>(okResult.Value);
            
            Assert.Equal(175.13m, response.Balance);
            Assert.True(response.Succeeded);
        }

        [Fact]
        public async Task Withdraw_WithAmountGreaterThanBalanceAmount_ReturnsConflict()
        {
            // Arrange
            _fixture.SeedTestData(); 
            var controller = CreateController();
            var withdrawRequest = new AccountTransactionRequestDTO
            {
                CustomerId = 5,
                AccountId = 17,
                Amount = 3000.00m
            };

            // Act
            var result = await controller.Withdraw(withdrawRequest);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Insufficient funds for this withdrawal", conflictResult.Value);
        }

        [Fact]
        public async Task Withdraw_MultipleWithdrawals_BalanceSubtractionAccumulates()
        {
            // Arrange
            _fixture.SeedTestData(); 
            var controller = CreateController();

            // Act - First deposit
            var result1 = await controller.Withdraw(new AccountTransactionRequestDTO
            {
                CustomerId = 5,
                AccountId = 17,
                Amount = 100m
            });
            var response1 = Assert.IsType<OkObjectResult>(result1).Value as AccountBalanceResponseDTO;
            Assert.NotNull(response1);
            Assert.Equal(2075.13m, response1.Balance);

            // Act - Second deposit
            var result2 = await controller.Withdraw(new AccountTransactionRequestDTO
            {
                CustomerId = 5,
                AccountId = 17,
                Amount = 200m
            });
            var response2 = Assert.IsType<OkObjectResult>(result2).Value as AccountBalanceResponseDTO;

            // Assert
            Assert.NotNull(response2);
            Assert.Equal(1875.13m, response2.Balance);
        }

// -------------------------------------------------------------------------------------------
        // COMBINED OPERATIONS
// -------------------------------------------------------------------------------------------


        [Fact]
        public async Task DepositThenWithdraw_WithValidAmounts_ReturnsCorrectFinalBalance()
        {
            // Arrange
            _fixture.SeedTestData(); 
            var controller = CreateController();

            // Act - Deposit
            var result1 = await controller.Deposit(new AccountTransactionRequestDTO
            {
                CustomerId = 5,
                AccountId = 17,
                Amount = 100m
            });
            var response1 = Assert.IsType<OkObjectResult>(result1).Value as AccountBalanceResponseDTO;
            Assert.NotNull(response1);
            Assert.Equal(2275.13m, response1.Balance);

            // Act - Withdraw
            var result2 = await controller.Withdraw(new AccountTransactionRequestDTO
            {
                CustomerId = 5,
                AccountId = 17,
                Amount = 200m
            });
            var response2 = Assert.IsType<OkObjectResult>(result2).Value as AccountBalanceResponseDTO;

            // Assert
            Assert.NotNull(response2);
            Assert.Equal(2075.13m, response2.Balance);
        }


        public void Dispose()
        {
            // Teardown: runs after every test
            // can put extra teardown logic here in the future
            _fixture.Dispose();
        }
    }
}
