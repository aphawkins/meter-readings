namespace MeterReadingsMvcApp.Controllers
{
	using System.Linq;
	using System.Threading.Tasks;
	using AutoMapper;
	using AutoMapper.QueryableExtensions;
	using MeterReadingsMvcApp.Models;
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
        public async Task<IActionResult> Index()
        {
			return View(await _service.MeterReading.ReadAsync<MeterReadingViewModel>(MapperConfig.Config));
        }

		// GET: MeterReadings/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			MeterReadingDto meterReading = (await _service.MeterReading.ReadAsync(x => x.Id == id.Value)).FirstOrDefault();
			if (meterReading == null)
			{
				return NotFound();
			}

			return View(new Mapper(MapperConfig.Config).Map<MeterReadingViewModel>(meterReading));
		}

		// GET: MeterReadings/Create
		public async Task<IActionResult> Create()
        {
            ViewData["AccountId"] = new SelectList(await _service.Account.ReadAsync(), "Id", "Id");
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
				await _service.MeterReading.CreateAsync(meterReading);
				return RedirectToAction(nameof(Index));
			}
			ViewData["AccountId"] = new SelectList(await _service.Account.ReadAsync(), "Id", "Id", meterReading.AccountId);
			return View(new Mapper(MapperConfig.Config).Map<MeterReadingViewModel>(meterReading));
		}

		// GET: MeterReadings/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			MeterReadingDto meterReading = (await _service.MeterReading.ReadAsync(x => x.Id == id.Value)).FirstOrDefault();
			if (meterReading == null)
			{
				return NotFound();
			}
			ViewData["AccountId"] = new SelectList(await _service.Account.ReadAsync(), "Id", "Id", meterReading.AccountId);
			return View(new Mapper(MapperConfig.Config).Map<MeterReadingViewModel>(meterReading));
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
				await _service.MeterReading.UpdateAsync(meterReading);
				return RedirectToAction(nameof(Index));
			}
			ViewData["AccountId"] = new SelectList(await _service.Account.ReadAsync(), "Id", "Id", meterReading.AccountId);
			return View(new Mapper(MapperConfig.Config).Map<MeterReadingViewModel>(meterReading));
		}

		// GET: MeterReadings/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			MeterReadingDto meterReading = (await _service.MeterReading.ReadAsync(x => x.Id == id.Value)).FirstOrDefault();
			if (meterReading == null)
			{
				return NotFound();
			}

			return View(new Mapper(MapperConfig.Config).Map<MeterReadingViewModel>(meterReading));
		}

		// POST: MeterReadings/Delete/5
		[HttpPost]
		[ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			await _service.MeterReading.DeleteAsync((await _service.MeterReading.ReadAsync(x => x.Id == id)).FirstOrDefault());
			return RedirectToAction(nameof(Index));
		}
	}
}
