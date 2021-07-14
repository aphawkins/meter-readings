namespace MeterReadingsApi.Controllers
{
	using MeterReadingsService;
	using MeterReadingsService.Dto;
	using Microsoft.AspNetCore.Mvc;
	using System.Linq;

	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		private readonly IMeterReadingsService _service;

		public AccountsController(IMeterReadingsService service)
		{
			_service = service;
		}

		// GET: api/Accounts
		[HttpGet]
		public ActionResult<IQueryable<AccountDto>> GetAccounts()
		{
			IQueryable<AccountDto> accounts = _service.Account.Read();
			return Ok(accounts);
		}

		// GET: api/Accounts/5
		[HttpGet("{id}")]
		public ActionResult<AccountDto> GetAccount(int id)
		{
			AccountDto account = _service.Account.Read(x => x.Id == id).FirstOrDefault();
			if (account == null)
			{
				return NotFound();
			}

			return Ok(account);
		}
	}
}