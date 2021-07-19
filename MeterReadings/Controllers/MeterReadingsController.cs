namespace MeterReadingsApi.Controllers
{
	using MeterReadingsDto;
	using MeterReadingsService;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using System.Collections.Generic;
	using System.IO;
	using System.Threading.Tasks;

	[ApiController]
	[Route("api/[controller]")]
	public class MeterReadingsController : ControllerBase
	{
		private readonly IMeterReadingsService _service;

		public MeterReadingsController(IMeterReadingsService service)
		{
			_service = service;
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> CreateMeterReading([FromBody] MeterReadingDto reading)
		{
			MeterReadingDto newReading;
			try
			{
				newReading = await _service.MeterReading.CreateAsync(reading);
			}
			catch
			{
				return BadRequest();
			}

			return Ok(newReading);
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

		[Route("csv-file")]
		[HttpPost]
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