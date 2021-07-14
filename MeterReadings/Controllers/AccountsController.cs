namespace MeterReadingsApi.Controllers
{
	using MeterReadingsService;
	using MeterReadingsService.Dto;
	using Microsoft.AspNetCore.Mvc;
	using System.Linq;
	using System.Threading.Tasks;

	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		private readonly IAccountService _service;

		public AccountsController(IAccountService service)
		{
			_service = service;
		}

		// GET: api/Accounts
		[HttpGet]
		public ActionResult<IQueryable<AccountDto>> GetAccounts()
		{
			IQueryable<AccountDto> accounts = _service.Read();
			return Ok(accounts);
		}

		// GET: api/Accounts/5
		[HttpGet("{id}")]
		public async Task<ActionResult<AccountDto>> GetAccount(int id)
		{
			AccountDto account = await _service.ReadAsync(id);
			if (account == null)
			{
				return NotFound();
			}

			return Ok(account);
		}
	}
}