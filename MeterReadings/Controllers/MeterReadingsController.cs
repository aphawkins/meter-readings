namespace MeterReadings.Controllers
{
	using MeterReadings.DTO;
	using MeterReadingsService;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
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
		public async Task<IActionResult> OnPostUploadAsync([FromServices] IMeterReadingService readingService, IFormFile file)
		{
			using StreamReader readingsReader = new(file.OpenReadStream());

			(int total, int successful) = await readingService.AddMeterReadingsAsync(readingsReader);

			readingsReader.Close();

			return Ok(new { successful, failed = total - successful });
		}
	}
}