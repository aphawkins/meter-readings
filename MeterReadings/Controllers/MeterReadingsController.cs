namespace MeterReadingsApi.Controllers
{
	using MeterReadingsService;
	using MeterReadingsService.Dto;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using System.Collections.Generic;
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
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<MeterReadingDto>>> GetMeterReadings()
		{
			IEnumerable<MeterReadingDto> readings = await _service.MeterReading.ReadAsync();
			return Ok(readings);
		}

		[HttpDelete]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult> DeleteMeterReadings()
		{
			await _service.MeterReading.DeleteAsync();
			return Ok(new { deleted = true });
		}

		[HttpPost(Name = "PostMeterReadingsCsvFile")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> OnPostUploadAsync(IFormFile file)
		{
			if (file == null)
			{
				return NotFound();
			}

			using StreamReader readingsReader = new(file.OpenReadStream());

			(int total, int successful) = await _service.MeterReading.AddMeterReadingsAsync(readingsReader);

			readingsReader.Close();

			return Ok(new { successful, failed = total - successful });
		}
	}
}