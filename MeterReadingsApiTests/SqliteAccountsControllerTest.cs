namespace MeterReadingsApiTests
{
	using System.Collections.Generic;
	using System.Linq;
	using MeterReadings.DTO;
	using MeterReadingsApi.Controllers;
	using MeterReadingsData;
	using MeterReadingsService;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;
	using Xunit;

	public class SqliteAccountsControllerTest : AccountsControllerTest
	{
		public SqliteAccountsControllerTest()
			: base(
				new DbContextOptionsBuilder<MainDbContext>()
					.UseSqlite("Filename=Test.db")
					.Options)
		{
		}

		private static T GetObjectResultContent<T>(ActionResult<T> result)
		{
			return (T)((ObjectResult)result.Result).Value;
		}

		[Fact]
		public void Can_get_accounts()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			AccountService service = new(context);
			AccountsController controller = new(service);

			// Act
			ActionResult<IQueryable<AccountDto>> actionResult = controller.GetAccounts();
			Assert.IsType<OkObjectResult>(actionResult.Result);
			List<AccountDto> accounts = GetObjectResultContent(actionResult).ToList();

			// Assert
			Assert.Equal(2, accounts.Count);
			Assert.Equal(1, accounts[0].AccountId);
			Assert.Equal("One", accounts[0].FirstName);
			Assert.Equal(2, accounts[1].AccountId);
			Assert.Equal("Two", accounts[1].FirstName);
		}

		[Fact]
		public void Can_get_account_by_id()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			AccountService service = new(context);
			AccountsController controller = new(service);

			// Act
			ActionResult<AccountDto> actionResult = controller.GetAccount(1).Result;
			Assert.IsType<OkObjectResult>(actionResult.Result);
			AccountDto account = GetObjectResultContent(actionResult);


			Assert.Equal(1, account.AccountId);
			Assert.Equal("One", account.FirstName);
		}
	}
}