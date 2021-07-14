namespace MeterReadingsServiceTests
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using MeterReadingsData;
	using MeterReadingsService;
	using MeterReadingsService.Dto;
	using Microsoft.EntityFrameworkCore;
	using Xunit;

	public class AccountsControllerTestsSqlite : ControllerTestsBase
	{
		public AccountsControllerTestsSqlite()
			: base(
				new DbContextOptionsBuilder<MainDbContext>()
					.UseSqlite($"Filename={nameof(AccountsControllerTestsSqlite)}.db")
					.Options)
		{
		}

		[Fact]
		public async Task Can_create_account()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IAccountService service = new AccountService(context);
			AccountDto newAccount = new()
			{
				Id = 3,
				FirstName = "new",
				LastName = "account",
			};

			// Act
			AccountDto account = await service.CreateAsync(newAccount);

			// Assert
			Assert.Equal(3, account.Id);
			Assert.Equal("new", account.FirstName);
			Assert.Equal("account", account.LastName);

			Assert.Equal(3, service.Read().Count());
		}

		[Fact]
		public void Can_read_all_accounts()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IAccountService service = new AccountService(context);

			// Act
			List<AccountDto> accounts = service.Read().ToList();

			// Assert
			Assert.Equal(2, accounts.Count);

			Assert.Equal(1, accounts[0].Id);
			Assert.Equal("One", accounts[0].FirstName);

			Assert.Equal(2, accounts[1].Id);
			Assert.Equal("Two", accounts[1].FirstName);
		}

		[Fact]
		public async Task Can_read_account_by_id()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IAccountService service = new AccountService(context);

			// Act
			AccountDto account = await service.ReadAsync(1);

			Assert.Equal(1, account.Id);
			Assert.Equal("One", account.FirstName);
			Assert.Equal("First", account.LastName);
		}

		[Fact]
		public async Task Can_update_account()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IAccountService service = new AccountService(context);
			AccountDto existing = await service.ReadAsync(1);
			existing.FirstName = "updated";
			existing.LastName = "account";

			// Act
			AccountDto account = await service.UpdateAsync(existing);

			// Assert
			Assert.Equal(1, account.Id);
			Assert.Equal("updated", account.FirstName);
			Assert.Equal("account", account.LastName);
		}

		[Fact]
		public async Task Can_delete_account_by_id()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IAccountService service = new AccountService(context);

			// Act
			bool isDeleted = await service.DeleteAsync(2);

			// Assert
			Assert.True(isDeleted);
			Assert.Equal(1, service.Read().Count());
		}

		[Fact]
		public async Task Cant_delete_no_account_by_id()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IAccountService service = new AccountService(context);

			// Act
			bool isDeleted = await service.DeleteAsync(3);

			// Assert
			Assert.False(isDeleted);
			Assert.Equal(2, service.Read().Count());
		}

		[Fact]
		public async Task Can_delete_all_accounts()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IAccountService service = new AccountService(context);

			// Act
			int count = await service.DeleteAsync();

			// Assert
			Assert.Equal(2, count);
			Assert.Empty(service.Read());
		}
	}
}