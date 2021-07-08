namespace MeterReadingsApi.Controllers
{
	using MeterReadings.DTO;
	using MeterReadingsService;
	using Microsoft.AspNetCore.Mvc;
	using System.Linq;
	using System.Threading.Tasks;

	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		// GET: api/Accounts
		[HttpGet]
		public IActionResult GetAccounts([FromServices] IAccountService service)
		{
			IQueryable<AccountDto> accounts = service.GetAllAccounts();
			return Ok(accounts);
		}

		// GET: api/Accounts/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetAccountsItem([FromServices] IAccountService service, int id)
		{
			AccountDto account = await service.GetAccountAsync(id);
			if (account == null)
			{
				return NotFound();
			}

			return Ok(account);
		}
	}
}