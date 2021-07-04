namespace MeterReadingsApi.Controllers
{
	using MeterReadingsApi.DTO;
	using MeterReadingsData;
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
			return Ok(_context.Accounts.Select(x => new AccountDto
			{
				AccountId = x.Id,
				FirstName = x.FirstName,
				LastName = x.LastName,
			}));
		}

		// GET: api/Accounts/5
		[HttpGet("{id}")]
		public ActionResult GetAccountsItem(int id)
		{
			var todoItem = _context.Accounts.Where(x => id == x.Id).Select(x => new AccountDto
			{
				AccountId = x.Id,
				FirstName = x.FirstName,
				LastName = x.LastName,
			});

			if (todoItem == null)
			{
				return NotFound();
			}

			return Ok(todoItem);
		}
	}
}