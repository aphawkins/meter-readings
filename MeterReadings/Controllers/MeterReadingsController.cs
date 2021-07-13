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
		private readonly IMeterReadingService _service;

		public MeterReadingsController(IMeterReadingService service)
		{
			_service = service;
		}

		[HttpGet]
		public IActionResult GetMeterReadings()
		{
			IQueryable<MeterReadingDto> readings = _service.GetAllMeterReadings();

			return Ok(readings);
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteMeterReadings()
		{
			int count = await _service.DeleteAllMeterReadingsAsync();

			return Ok(new { deleted = count });
		}

		[HttpPost(Name = "PostMeterReadingsCsvFile")]
		public async Task<IActionResult> OnPostUploadAsync(IFormFile file)
		{
			using StreamReader readingsReader = new(file.OpenReadStream());

			(int total, int successful) = await _service.AddMeterReadingsAsync(readingsReader);

			readingsReader.Close();

			return Ok(new { successful, failed = total - successful });
		}
	}
}