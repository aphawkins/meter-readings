namespace MeterReadings.Api.Controllers
{
	using MeterReadings.Dto;
	using MeterReadings.Service;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;

	[ApiController]
	[Route("api/meterreadings")]
	public class MeterReadingsController : ControllerBase
	{
		private readonly IMeterReadingsService _service;

		public MeterReadingsController(IMeterReadingsService service)
		{
			_service = service;
		}

		// POST: api/meterreadings
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

		// GET: api/meterreadings
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<MeterReadingDto>>> GetMeterReadings()
		{
			IEnumerable<MeterReadingDto> readings = await _service.MeterReading.ReadAsync();
			return Ok(readings);
		}

		// GET: api/meterreadings/{id}
		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<MeterReadingDto>> GetMeterReading(int id)
		{
			MeterReadingDto reading = (await _service.MeterReading.ReadAsync(x => x.Id == id)).FirstOrDefault();
			if (reading == null)
			{
				return NotFound();
			}

			return Ok(reading);
		}

		// PUT: api/meterreadings
		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<MeterReadingDto>> UpdateMeterReading([FromBody] MeterReadingDto readingDto)
		{
			MeterReadingDto newReading;

			try
			{
				newReading = await _service.MeterReading.UpdateAsync(readingDto);
			}
			catch (MeterReadingsServiceException)
			{
				return NotFound();
			}

			return Ok(newReading);
		}

		// Delete: api/meterreadings
		[HttpDelete]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult> DeleteMeterReadings()
		{
			await _service.MeterReading.DeleteAsync();
			return Ok(new { deleted = true });
		}

		// Delete: api/meterreadings/{id}
		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> DeleteMeterReading([FromRoute] int id)
		{
			MeterReadingDto reading = (await _service.MeterReading.ReadAsync(x => x.Id == id)).FirstOrDefault();
			if (reading == null)
			{
				return NotFound();
			}

			try
			{
				await _service.MeterReading.DeleteAsync(reading);
			}
			catch (MeterReadingsServiceException)
			{
				return NotFound();
			}

			return Ok();
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