namespace MeterReadingsMvcApp.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using MeterReadingsService;
	using MeterReadingsService.Dto;
	using System.Linq;
	using MeterReadingsMvcApp.Models;
	using AutoMapper.QueryableExtensions;
	using AutoMapper;
	using System.Threading.Tasks;

	public class AccountsController : Controller
    {
		private readonly IMeterReadingsService _service;

		public AccountsController(IMeterReadingsService service)
		{
			_service = service;
		}

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
			return View(await _service.Account.ReadAsync<AccountViewModel>(MapperConfig.Config));
        }

		// GET: Accounts/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			AccountDto account = (await _service.Account.ReadAsync(x => x.Id == id.Value)).FirstOrDefault();
			if (account == null)
			{
				return NotFound();
			}

			return View(new Mapper(MapperConfig.Config).Map<AccountViewModel>(account));
		}

		// GET: Accounts/Create
		public IActionResult Create()
        {
            return View();
        }

		// POST: Accounts/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,FirstName,LastName")] AccountDto account)
		{
			if (ModelState.IsValid)
			{
				await _service.Account.CreateAsync(account);
				return RedirectToAction(nameof(Index));
			}
			return View(new Mapper(MapperConfig.Config).Map<AccountViewModel>(account));
		}

		// GET: Accounts/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			AccountDto account = (await _service.Account.ReadAsync(x => x.Id == id.Value)).FirstOrDefault();
			if (account == null)
			{
				return NotFound();
			}
			return View(new Mapper(MapperConfig.Config).Map<AccountViewModel>(account));
		}

		// POST: Accounts/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName")] AccountDto account)
		{
			if (id != account.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				await _service.Account.UpdateAsync(account);
				return RedirectToAction(nameof(Index));
			}
			return View(new Mapper(MapperConfig.Config).Map<AccountViewModel>(account));
		}

		// GET: Accounts/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			AccountDto account = (await _service.Account.ReadAsync(x => x.Id == id.Value)).FirstOrDefault();
			if (account == null)
			{
				return NotFound();
			}

			return View(new Mapper(MapperConfig.Config).Map<AccountViewModel>(account));
		}

		// POST: Accounts/Delete/5
		[HttpPost]
		[ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			await _service.Account.DeleteAsync((await _service.Account.ReadAsync(x => x.Id == id)).FirstOrDefault());
			return RedirectToAction(nameof(Index));
		}
	}
}
