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
		public void Can_get_account_by_id()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);
			AccountsController controller = new(service);

			// Act
			ActionResult<AccountDto> actionResult = controller.GetAccount(1).Result;
			Assert.IsType<OkObjectResult>(actionResult.Result);
			AccountDto account = GetObjectResultContent(actionResult);


			Assert.Equal(1, account.Id);
			Assert.Equal("One", account.FirstName);
		}
	}
}