namespace MeterReadingsApi.Controllers
{
	using MeterReadings.DTO;
	using MeterReadingsData;
	using MeterReadingsService;
	using Microsoft.AspNetCore.Mvc;
	using System.Linq;
	using System.Threading.Tasks;

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
		public IActionResult GetAccounts()
		{
			IAccountService service = new AccountService(_context);
			IQueryable<AccountDto> accounts = service.GetAllAccounts();
			return Ok(accounts);
		}

		// GET: api/Accounts/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetAccountsItem(int id)
		{
			IAccountService service = new AccountService(_context);
			AccountDto account = await service.GetAccountAsync(id);
			if (account == null)
			{
				return NotFound();
			}

			return Ok(account);
		}
	}
}