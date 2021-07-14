namespace MeterReadingsApi.Controllers
{
	using MeterReadingsService;
	using MeterReadingsService.Dto;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;

	[ApiController]
	[Route("meter-reading-uploads")]
	public class MeterReadingsController : ControllerBase
	{
		private readonly IMeterReadingsService _service;

		public MeterReadingsController(IMeterReadingsService service)
		{
			_service = service;
		}

		[HttpGet]
		public ActionResult<IQueryable<MeterReadingDto>> GetMeterReadings()
		{
			IQueryable<MeterReadingDto> readings = _service.MeterReading.Read();
			return Ok(readings);
		}

		[HttpDelete]
		public ActionResult DeleteMeterReadings()
		{
			_service.MeterReading.Delete();
			return Ok(new { deleted = true });
		}

		[HttpPost(Name = "PostMeterReadingsCsvFile")]
		public async Task<ActionResult> OnPostUploadAsync(IFormFile file)
		{
			using StreamReader readingsReader = new(file.OpenReadStream());

			(int total, int successful) = await _service.MeterReading.AddMeterReadingsAsync(readingsReader);

			readingsReader.Close();

			return Ok(new { successful, failed = total - successful });
		}
	}
}