namespace MeterReadings.Controllers
{
	using MeterReadings.DTO;
	using MeterReadingsData;
	using MeterReadingsData.Models;
	using MeterReadingsService;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;
	using System;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;

	[ApiController]
	[Route("meter-reading-uploads")]
	public class MeterReadingsController : ControllerBase
	{
		private readonly MainDbContext _context;

		public MeterReadingsController(MainDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult GetMeterReadings()
		{
			IQueryable<MeterReadingDto> readings = MeterReadingService.MapMeterReadingToDto(_context.MeterReadings);

			return Ok(readings);
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteMeterReadings()
		{
			foreach (MeterReading entity in _context.MeterReadings)
			{
				_context.MeterReadings.Remove(entity);
			}

			int count = await _context.SaveChangesAsync();

			return Ok(new { deleted = count });
		}

		[HttpPost(Name = "PostMeterReadingsCsvFile")]
		public async Task<IActionResult> OnPostUploadAsync(IFormFile file)
		{
			int total = 0;
			string line;
			using StreamReader readingsReader = new(file.OpenReadStream());

			// Assume header on first line
			await readingsReader.ReadLineAsync();

			while ((line = await readingsReader.ReadLineAsync()) != null)
			{
				string[] details = line.Split(',');

				if (details.Length >= 3 &&
					int.TryParse(details[0], out int accountId) &&
					DateTime.TryParse(details[1], out DateTime readingDT) &&
					int.TryParse(details[2], out int readingValue) &&
					readingValue >= 0 &&
					readingValue < 100000)
				{
					// Does account exist?
					Account account = await _context.Accounts.FindAsync(accountId);
					// Does reading already exist?
					MeterReading reading = await _context.MeterReadings.FirstOrDefaultAsync(x => x.AccountId == accountId && x.MeterReadingDateTime == readingDT);

					if (account != null &&
						reading == null)
					{
						await _context.MeterReadings.AddAsync(
							new MeterReading
							{
								AccountId = accountId,
								MeterReadingDateTime = readingDT,
								MeterReadingValue = readingValue,
							});
					}
				}

				total++;
			}

			readingsReader.Close();

			int successful = await _context.SaveChangesAsync();

			return Ok(new { successful, failed = total - successful });
		}
	}
}