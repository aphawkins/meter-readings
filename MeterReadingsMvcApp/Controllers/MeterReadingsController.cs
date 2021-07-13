namespace MeterReadingsMvcApp.Controllers
{
	using System.Threading.Tasks;
	using MeterReadings.DTO;
	using MeterReadingsService;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Rendering;

	public class MeterReadingsController : Controller
    {
		private readonly IAccountService _accountService;
		private readonly IMeterReadingService _readingService;

		public MeterReadingsController(IAccountService accountService, IMeterReadingService readingService)
        {
			_accountService = accountService;
			_readingService = readingService;
        }

        // GET: MeterReadings
        public IActionResult Index()
        {
			return View(_readingService.Read());
        }

        // GET: MeterReadings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			MeterReadingDto meterReading = await _readingService.ReadAsync(id.Value);
            if (meterReading == null)
            {
                return NotFound();
            }

            return View(meterReading);
        }

        // GET: MeterReadings/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_accountService.Read(), "Id", "Id");
            return View();
        }

        // POST: MeterReadings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountId,MeterReadingDateTime,MeterReadingValue")] MeterReadingDto meterReading)
        {
            if (ModelState.IsValid)
            {
				await _readingService.CreateAsync(meterReading);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_accountService.Read(), "Id", "Id", meterReading.AccountId);
            return View(meterReading);
        }

        // GET: MeterReadings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			MeterReadingDto meterReading = await _readingService.ReadAsync(id.Value);
			if (meterReading == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_accountService.Read(), "Id", "Id", meterReading.AccountId);
            return View(meterReading);
        }

        // POST: MeterReadings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId,MeterReadingDateTime,MeterReadingValue")] MeterReadingDto meterReading)
        {
            if (id != meterReading.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
				await _readingService.UpdateAsync(meterReading);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_accountService.Read(), "Id", "Id", meterReading.AccountId);
            return View(meterReading);
        }

        // GET: MeterReadings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			MeterReadingDto meterReading = await _readingService.ReadAsync(id.Value);
            if (meterReading == null)
            {
                return NotFound();
            }

            return View(meterReading);
        }

        // POST: MeterReadings/Delete/5
        [HttpPost]
		[ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
			await _readingService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
