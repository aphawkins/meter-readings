namespace MeterReadingsMvcApp.Controllers
{
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Mvc;
	using MeterReadingsService;
	using MeterReadings.DTO;

	public class AccountsController : Controller
    {
		private readonly IAccountService _service;

		public AccountsController(IAccountService service)
		{
			_service = service;
		}

        // GET: Accounts
        public IActionResult Index()
        {
			return View(_service.Read());
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _service.ReadAsync(id.Value);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
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
				await _service.CreateAsync(account);
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _service.ReadAsync(id.Value);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
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
                await _service.UpdateAsync(account);
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			AccountDto account = await _service.ReadAsync(id.Value);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost]
		[ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
			await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
