namespace MeterReadings.Controllers
{
	using MeterReadings.DTO;
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
		[HttpGet]
		public IActionResult GetMeterReadings([FromServices] IMeterReadingService service)
		{
			IQueryable<MeterReadingDto> readings = service.GetAllMeterReadings();

			return Ok(readings);
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteMeterReadings([FromServices] IMeterReadingService service)
		{
			int count = await service.DeleteAllMeterReadingsAsync();

			return Ok(new { deleted = count });
		}

		[HttpPost(Name = "PostMeterReadingsCsvFile")]
		public async Task<IActionResult> OnPostUploadAsync([FromServices] IAccountService accountService, [FromServices] IMeterReadingService readingService, IFormFile file)
		{
			int successful = 0;
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
					AccountDto account = await accountService.GetAccountAsync(accountId);

					// Does reading already exist?
					MeterReadingDto reading = await readingService.GetMeterReadingAsync(accountId, readingDT);

					if (account != null &&
						reading == null)
					{
						MeterReadingDto newReading = await readingService.AddMeterReadingAsync(accountId, readingDT, readingValue);
						if (newReading != null)
						{
							successful++;
						}
					}
				}

				total++;
			}

			readingsReader.Close();

			return Ok(new { successful, failed = total - successful });
		}
	}
}