namespace MeterReadings.Controllers
{
	using MeterReadings.DTO;
	using MeterReadingsData;
	using MeterReadingsService;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
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
			IMeterReadingService service = new MeterReadingService(_context);
			IQueryable<MeterReadingDto> readings = service.GetAllMeterReadings();

			return Ok(readings);
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteMeterReadings()
		{
			IMeterReadingService service = new MeterReadingService(_context);
			int count = await service.DeleteAllMeterReadingsAsync();

			return Ok(new { deleted = count });
		}

		[HttpPost(Name = "PostMeterReadingsCsvFile")]
		public async Task<IActionResult> OnPostUploadAsync(IFormFile file)
		{
			IAccountService accountsService = new AccountService(_context);
			IMeterReadingService readingsService = new MeterReadingService(_context);

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
					AccountDto account = await accountsService.GetAccountAsync(accountId);

					// Does reading already exist?
					MeterReadingDto reading = await readingsService.GetMeterReadingAsync(accountId, readingDT);

					if (account != null &&
						reading == null)
					{
						await readingsService.AddMeterReadingAsync(accountId, readingDT, readingValue);
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