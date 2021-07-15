﻿namespace MeterReadingsServiceTests
{
	using System.Collections.Generic;
	using System.Linq;
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
		public void Can_create_account()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);
			AccountDto newAccount = new()
			{
				Id = 3,
				FirstName = "new",
				LastName = "account",
			};

			// Act
			AccountDto account = service.Account.Create(newAccount);

			// Assert
			Assert.Equal(3, account.Id);
			Assert.Equal("new", account.FirstName);
			Assert.Equal("account", account.LastName);

			Assert.Equal(3, service.Account.Read().Count());
		}

		[Fact]
		public void Can_read_all_accounts()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);

			// Act
			List<AccountDto> accounts = service.Account.Read().ToList();

			// Assert
			Assert.Equal(2, accounts.Count);

			Assert.Equal(1, accounts[0].Id);
			Assert.Equal("One", accounts[0].FirstName);

			Assert.Equal(2, accounts[1].Id);
			Assert.Equal("Two", accounts[1].FirstName);
		}

		[Fact]
		public void Can_read_account_by_id()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);

			// Act
			AccountDto account = service.Account.Read(x => x.Id == 1).FirstOrDefault();

			Assert.Equal(1, account.Id);
			Assert.Equal("One", account.FirstName);
			Assert.Equal("First", account.LastName);
		}

		[Fact]
		public void Can_update_account()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);
			AccountDto existing = service.Account.Read(x => x.Id == 1).FirstOrDefault();
			existing.FirstName = "updated";
			existing.LastName = "account";

			// Act
			AccountDto account = service.Account.Update(existing);

			// Assert
			Assert.Equal(1, account.Id);
			Assert.Equal("updated", account.FirstName);
			Assert.Equal("account", account.LastName);
		}

		[Fact]
		public void Can_delete_account_by_id()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);

			// Act
			service.Account.Delete(service.Account.Read(x => x.Id == 2).FirstOrDefault());

			// Assert
			Assert.Equal(1, service.Account.Read().Count());
		}

		[Fact]
		public void Cant_delete_no_account_by_id()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);

			// Act
			service.Account.Delete(null);

			// Assert
			Assert.Equal(2, service.Account.Read().Count());
		}

		[Fact]
		public void Can_delete_all_accounts()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);

			// Act
			service.Account.Delete();

			// Assert
			Assert.Empty(service.Account.Read());
		}
	}
}