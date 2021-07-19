namespace MeterReadingsServiceTests
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using MeterReadingsData;
	using MeterReadingsDto;
	using MeterReadingsService;
	using MeterReadingsTestLibrary;
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
			IMeterReadingsService service = new MeterReadingsService(context);
			AccountDto newAccount = new()
			{
				Id = 3,
				FirstName = "Three",
				LastName = "Third",
			};

			// Act
			AccountDto account = await service.Account.CreateAsync(newAccount);

			// Assert
			Assert.Equal(3, (await service.Account.ReadAsync()).Count());
			Assert.Equal(3, account.Id);
			Assert.Equal("Three", account.FirstName);
			Assert.Equal("Third", account.LastName);
		}

		[Fact]
		public async Task Cant_create_duplicate_account_id()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);
			AccountDto newAccount = new()
			{
				Id = 2,
				FirstName = "Two",
				LastName = "Second",
			};

			// Act & Assert
			await Assert.ThrowsAsync<MeterReadingsServiceException>(() => service.Account.CreateAsync(newAccount));
		}

		[Fact]
		public async Task Can_read_all_accounts()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);

			// Act
			List<AccountDto> accounts = (await service.Account.ReadAsync()).ToList();

			// Assert
			Assert.Equal(2, accounts.Count);

			Assert.Equal(1, accounts[0].Id);
			Assert.Equal("One", accounts[0].FirstName);
			Assert.Equal("First", accounts[0].LastName);

			Assert.Equal(2, accounts[1].Id);
			Assert.Equal("Two", accounts[1].FirstName);
			Assert.Equal("Second", accounts[1].LastName);
		}

		[Fact]
		public async Task Can_read_account_by_id()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);

			// Act
			AccountDto account = (await service.Account.ReadAsync(x => x.Id == 1)).FirstOrDefault();

			Assert.Equal(1, account.Id);
			Assert.Equal("One", account.FirstName);
			Assert.Equal("First", account.LastName);
		}

		[Fact]
		public async Task Can_update_account()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);
			AccountDto existing = (await service.Account.ReadAsync(x => x.Id == 1)).FirstOrDefault();
			existing.FirstName = "updated";
			existing.LastName = "account";

			// Act
			AccountDto account = await service.Account.UpdateAsync(existing);

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
			IMeterReadingsService service = new MeterReadingsService(context);

			// Act
			await service.Account.DeleteAsync((await service.Account.ReadAsync(x => x.Id == 2)).FirstOrDefault());

			// Assert
			Assert.Single(await service.Account.ReadAsync());
		}

		[Fact]
		public async Task Cant_delete_no_account_by_id()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);

			// Act
			await service.Account.DeleteAsync(null);

			// Assert
			Assert.Equal(2, (await service.Account.ReadAsync()).Count());
		}

		[Fact]
		public async Task Can_delete_all_accounts()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);

			// Act
			await service.Account.DeleteAsync();

			// Assert
			Assert.Empty(await service.Account.ReadAsync());
		}
	}
}