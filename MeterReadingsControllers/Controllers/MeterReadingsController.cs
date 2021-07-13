namespace MeterReadingsMvcApp.Controllers
{
	using System.Linq;
	using System.Threading.Tasks;
	using MeterReadingsData;
	using MeterReadingsData.Models;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Rendering;
	using Microsoft.EntityFrameworkCore;

	public class MeterReadingsController : Controller
	{
        private readonly MainDbContext _context;

        public MeterReadingsController(MainDbContext context)
        {
            _context = context;
        }

        // GET: MeterReadings
        public async Task<IActionResult> Index()
        {
            var mainDbContext = _context.MeterReadings.Include(m => m.MyAccount);
            return View(await mainDbContext.ToListAsync());
        }

        // GET: MeterReadings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
				return new NotFoundResult();
			}

            var meterReading = await _context.MeterReadings
                .Include(m => m.MyAccount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meterReading == null)
            {
                return new NotFoundResult();
            }

            return View(meterReading);
        }

        // GET: MeterReadings/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id");
            return View();
        }

        // POST: MeterReadings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountId,MeterReadingDateTime,MeterReadingValue")] MeterReading meterReading)
        {
            if (ModelState.IsValid)
            {
                _context.Add(meterReading);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", meterReading.AccountId);
            return View(meterReading);
        }

        // GET: MeterReadings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundResult();
            }

            var meterReading = await _context.MeterReadings.FindAsync(id);
            if (meterReading == null)
            {
                return new NotFoundResult();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", meterReading.AccountId);
            return View(meterReading);
        }

        // POST: MeterReadings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId,MeterReadingDateTime,MeterReadingValue")] MeterReading meterReading)
        {
            if (id != meterReading.Id)
            {
                return new NotFoundResult();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meterReading);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeterReadingExists(meterReading.Id))
                    {
                        return new NotFoundResult();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", meterReading.AccountId);
            return View(meterReading);
        }

        // GET: MeterReadings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
				return new NotFoundResult();
			}

            var meterReading = await _context.MeterReadings
                .Include(m => m.MyAccount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meterReading == null)
            {
				return new NotFoundResult();
			}

            return View(meterReading);
        }

        // POST: MeterReadings/Delete/5
        [HttpPost]
		[ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meterReading = await _context.MeterReadings.FindAsync(id);
            _context.MeterReadings.Remove(meterReading);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeterReadingExists(int id)
        {
            return _context.MeterReadings.Any(e => e.Id == id);
        }
    }
}
