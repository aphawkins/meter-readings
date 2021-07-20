namespace MeterReadingsApi.Controllers
{
	using MeterReadingsDto;
	using MeterReadingsService;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	[Route("api/accounts")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		private readonly IMeterReadingsService _service;

		public AccountsController(IMeterReadingsService service)
		{
			_service = service;
		}

		// GET: api/accounts
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts()
		{
			IEnumerable<AccountDto> accounts = await _service.Account.ReadAsync();
			return Ok(accounts);
		}

		// GET: api/accounts/5
		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<AccountDto>> GetAccount(int id)
		{
			AccountDto account = (await _service.Account.ReadAsync(x => x.Id == id)).FirstOrDefault();
			if (account == null)
			{
				return NotFound();
			}

			return Ok(account);
		}

		// POST: api/accounts
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<ActionResult<AccountDto>> CreateAccount([FromBody] AccountDto accountDto)
		{
			AccountDto newAccount;

			try
			{
				newAccount = await _service.Account.CreateAsync(accountDto);
			}
			catch (MeterReadingsServiceException)
			{
				return Conflict();
			}

			return Ok(newAccount);
		}

		// PUT: api/accounts
		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<AccountDto>> UpdateAccount([FromBody] AccountDto accountDto)
		{
			AccountDto newAccount;
			IEnumerable<AccountDto> accounts = await _service.Account.ReadAsync(x => x.Id == accountDto.Id);
			if (accounts == null)
			{
				return BadRequest();
			}

			try
			{
				newAccount = await _service.Account.UpdateAsync(accountDto);
			}
			catch (MeterReadingsServiceException)
			{
				return NotFound();
			}

			return Ok(newAccount);
		}
	}
}