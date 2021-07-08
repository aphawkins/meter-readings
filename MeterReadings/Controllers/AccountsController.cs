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
			IAccountService service = new AccountService(_context);
			IQueryable<AccountDto> accounts = service.GetAllAccounts();
			return Ok(accounts);
		}

		// GET: api/Accounts/5
		[HttpGet("{id}")]
		public ActionResult GetAccountsItem(int id)
		{
			IAccountService service = new AccountService(_context);
			IQueryable<AccountDto> accounts = service.GetAccountById(id);
			if (accounts == null)
			{
				return NotFound();
			}

			return Ok(accounts);
		}
	}
}