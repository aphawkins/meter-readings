namespace MeterReadingsApi.Controllers
{
	using MeterReadings.DTO;
	using MeterReadingsData;
	using MeterReadingsService;
	using Microsoft.AspNetCore.Mvc;
	using System.Linq;

	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		private readonly MainDbContext _context;

		public AccountsController(MainDbContext context)
		{
			_context = context;
		}

		// GET: api/Accounts
		[HttpGet]
		public ActionResult GetAccounts()
		{
			IQueryable<AccountDto> accounts = AccountService.MapAccountToDto(_context.Accounts);
			return Ok(accounts);
		}

		// GET: api/Accounts/5
		[HttpGet("{id}")]
		public ActionResult GetAccountsItem(int id)
		{
			IQueryable<AccountDto> accounts = AccountService.MapAccountToDto(_context.Accounts.Where(x => id == x.Id));
			if (accounts == null)
			{
				return NotFound();
			}

			return Ok(accounts);
		}
	}
}