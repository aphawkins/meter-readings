namespace MeterReadingsApiTests
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using MeterReadingsApi.Controllers;
	using MeterReadingsData;
	using MeterReadingsDto;
	using MeterReadingsService;
	using MeterReadingsTestLibrary;
	using Microsoft.AspNetCore.Mvc;
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
			AccountsController controller = new(service);
			AccountDto newAccount = new()
			{
				Id = 3,
				FirstName = "Three",
				LastName = "Third",
			};

			// Act
			ActionResult<AccountDto> actionResult = await controller.CreateAccount(newAccount);
			Assert.IsType<OkObjectResult>(actionResult.Result);
			AccountDto account = GetObjectResultContent(actionResult);

			// Assert
			Assert.Equal(3, GetObjectResultContent(await controller.GetAccounts()).Count());
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
			AccountsController controller = new(service);
			AccountDto newAccount = new()
			{
				Id = 2,
				FirstName = "Two",
				LastName = "Second",
			};

			// Act & Assert
			ActionResult<AccountDto> actionResult = await controller.CreateAccount(newAccount);
			Assert.IsType<ConflictResult>(actionResult.Result);
		}

		[Fact]
		public async Task Can_get_accounts()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);
			AccountsController controller = new(service);

			// Act
			ActionResult<IEnumerable<AccountDto>> actionResult = await controller.GetAccounts();
			Assert.IsType<OkObjectResult>(actionResult.Result);
			List<AccountDto> accounts = GetObjectResultContent(actionResult).ToList();

			// Assert
			Assert.Equal(2, accounts.Count);

			Assert.Equal(1, accounts[0].Id);
			Assert.Equal("One", accounts[0].FirstName);

			Assert.Equal(2, accounts[1].Id);
			Assert.Equal("Two", accounts[1].FirstName);
		}

		[Fact]
		public async Task Can_get_account_by_id()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);
			AccountsController controller = new(service);

			// Act
			ActionResult<AccountDto> actionResult = await controller.GetAccount(1);
			Assert.IsType<OkObjectResult>(actionResult.Result);
			AccountDto account = GetObjectResultContent(actionResult);

			// Assert
			Assert.Equal(1, account.Id);
			Assert.Equal("One", account.FirstName);
		}

		[Fact]
		public async Task Cant_get_account_by_no_id()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);
			AccountsController controller = new(service);

			// Act & Assert
			ActionResult<AccountDto> actionResult = await controller.GetAccount(3);
			Assert.IsType<NotFoundResult>(actionResult.Result);
		}

		[Fact]
		public async Task Can_update_account()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);
			AccountsController controller = new(service);

			AccountDto existing = (await service.Account.ReadAsync(x => x.Id == 1)).FirstOrDefault();
			existing.FirstName = "updated";
			existing.LastName = "account";

			// Act
			ActionResult<AccountDto> actionResult = await controller.UpdateAccount(existing);
			Assert.IsType<OkObjectResult>(actionResult.Result);
			AccountDto account = GetObjectResultContent(actionResult);

			// Assert
			Assert.Equal(1, account.Id);
			Assert.Equal("updated", account.FirstName);
			Assert.Equal("account", account.LastName);
		}

		[Fact]
		public async Task Cant_update_no_account()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);
			AccountsController controller = new(service);

			AccountDto existing = new()
			{
				Id = 3,
				FirstName = "updated",
				LastName = "account",
			};

			// Act & Assert
			ActionResult<AccountDto> actionResult = await controller.UpdateAccount(existing);
			Assert.IsType<NotFoundResult>(actionResult.Result);
		}

		[Fact]
		public async Task Can_delete_account_by_id()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);
			AccountsController controller = new(service);

			// Act
			ActionResult actionResult = await controller.DeleteAccount(1);
			Assert.IsType<OkResult>(actionResult);

			// Assert
			Assert.Single(GetObjectResultContent(await controller.GetAccounts()));
			ActionResult<AccountDto> getActionResult = controller.GetAccount(1).Result;
			Assert.IsType<NotFoundResult>(getActionResult.Result);
		}

		[Fact]
		public async Task Cant_delete_no_account_by_id()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);
			AccountsController controller = new(service);

			// Act & Assert
			ActionResult actionResult = await controller.DeleteAccount(3);
			Assert.IsType<NotFoundResult>(actionResult);
		}
	}
}