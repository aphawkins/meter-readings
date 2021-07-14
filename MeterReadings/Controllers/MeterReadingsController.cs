namespace MeterReadingsApi.Controllers
{
	using MeterReadingsService;
	using MeterReadingsService.Dto;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using System.IO;
	using System.Linq;
	using System.Text.Json;
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
		public ActionResult<IQueryable<MeterReadingDto>> GetMeterReadings()
		{
			IQueryable<MeterReadingDto> readings = _service.Read();
			return Ok(readings);
		}

		[HttpDelete]
		public async Task<ActionResult> DeleteMeterReadings()
		{
			int count = await _service.DeleteAsync();
			return Ok(new { deleted = count });
		}

		[HttpPost(Name = "PostMeterReadingsCsvFile")]
		public async Task<ActionResult> OnPostUploadAsync(IFormFile file)
		{
			using StreamReader readingsReader = new(file.OpenReadStream());

			(int total, int successful) = await _service.AddMeterReadingsAsync(readingsReader);

			readingsReader.Close();

			return Ok(new { successful, failed = total - successful });
		}
	}
}