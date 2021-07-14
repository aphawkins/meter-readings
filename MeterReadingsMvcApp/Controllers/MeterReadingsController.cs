namespace MeterReadingsMvcApp.Controllers
{
	using System.Linq;
	using MeterReadingsService;
	using MeterReadingsService.Dto;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Rendering;

	public class MeterReadingsController : Controller
    {
		private readonly IMeterReadingsService _service;

		public MeterReadingsController(IMeterReadingsService service)
        {
			_service = service;
        }

        // GET: MeterReadings
        public IActionResult Index()
        {
			return View(_service.MeterReading.Read());
        }

		// GET: MeterReadings/Details/5
		public IActionResult Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			MeterReadingDto meterReading = _service.MeterReading.Read(x => x.Id == id.Value).FirstOrDefault();
			if (meterReading == null)
			{
				return NotFound();
			}

			return View(meterReading);
		}

		// GET: MeterReadings/Create
		public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_service.Account.Read(), "Id", "Id");
            return View();
        }

		// POST: MeterReadings/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create([Bind("Id,AccountId,MeterReadingDateTime,MeterReadingValue")] MeterReadingDto meterReading)
		{
			if (ModelState.IsValid)
			{
				_service.MeterReading.Create(meterReading);
				return RedirectToAction(nameof(Index));
			}
			ViewData["AccountId"] = new SelectList(_service.Account.Read(), "Id", "Id", meterReading.AccountId);
			return View(meterReading);
		}

		// GET: MeterReadings/Edit/5
		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			MeterReadingDto meterReading = _service.MeterReading.Read(x => x.Id == id.Value).FirstOrDefault();
			if (meterReading == null)
			{
				return NotFound();
			}
			ViewData["AccountId"] = new SelectList(_service.Account.Read(), "Id", "Id", meterReading.AccountId);
			return View(meterReading);
		}

		// POST: MeterReadings/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(int id, [Bind("Id,AccountId,MeterReadingDateTime,MeterReadingValue")] MeterReadingDto meterReading)
		{
			if (id != meterReading.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				_service.MeterReading.Update(meterReading);
				return RedirectToAction(nameof(Index));
			}
			ViewData["AccountId"] = new SelectList(_service.Account.Read(), "Id", "Id", meterReading.AccountId);
			return View(meterReading);
		}

		// GET: MeterReadings/Delete/5
		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			MeterReadingDto meterReading = _service.MeterReading.Read(x => x.Id == id.Value).FirstOrDefault();
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
		public IActionResult DeleteConfirmed(int id)
		{
			_service.MeterReading.Delete(_service.MeterReading.Read(x => x.Id == id).FirstOrDefault());
			return RedirectToAction(nameof(Index));
		}
	}
}
