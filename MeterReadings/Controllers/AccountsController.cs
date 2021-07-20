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

		// GET: api/accounts
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts()
		{
			IEnumerable<AccountDto> accounts = await _service.Account.ReadAsync();
			return Ok(accounts);
		}

		// GET: api/accounts/{id}
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

		// PUT: api/accounts
		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<AccountDto>> UpdateAccount([FromBody] AccountDto accountDto)
		{
			AccountDto newAccount;

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

		// Delete: api/accounts/{id}
		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> DeleteAccount([FromRoute] int id)
		{
			AccountDto account = (await _service.Account.ReadAsync(x => x.Id == id)).FirstOrDefault();
			if (account == null)
			{
				return NotFound();
			}

			try
			{
				await _service.Account.DeleteAsync(account);
			}
			catch (MeterReadingsServiceException)
			{
				return NotFound();
			}

			return Ok();
		}
	}
}