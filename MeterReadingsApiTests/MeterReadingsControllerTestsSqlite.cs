namespace MeterReadingsApiTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using MeterReadingsApi.Controllers;
	using MeterReadingsData;
	using MeterReadingsDto;
	using MeterReadingsService;
	using MeterReadingsTestLibrary;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using Xunit;

	public class MeterReadingsControllerTestsSqlite : ControllerTestsBase
	{
		public MeterReadingsControllerTestsSqlite()
			: base(
				new DbContextOptionsBuilder<MainDbContext>()
					.UseSqlite($"Filename={nameof(MeterReadingsControllerTestsSqlite)}.db")
					.Options)
		{
		}

		[Fact]
		public async Task Can_create_meter_reading()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);
			MeterReadingsController controller = new(service);
			MeterReadingDto newReading = new()
			{
				Id = 3,
				AccountId = 1,
				MeterReadingDateTime = new DateTime(2003, 3, 3),
				MeterReadingValue = 3333
			};

			// Act
			ActionResult<MeterReadingDto> actionResult = await controller.CreateMeterReading(newReading);
			Assert.IsType<OkObjectResult>(actionResult.Result);
			MeterReadingDto reading = GetObjectResultContent(actionResult);

			// Assert
			Assert.Equal(3, GetObjectResultContent(await controller.GetMeterReadings()).Count());

			Assert.Equal(3, reading.Id);
			Assert.Equal(1, reading.AccountId);
			Assert.Equal(new DateTime(2003, 3, 3), reading.MeterReadingDateTime);
			Assert.Equal(3333, reading.MeterReadingValue);
		}

		[Fact]
		public async Task Cant_create_meter_reading_with_no_account()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);
			MeterReadingsController controller = new(service);
			MeterReadingDto newReading = new()
			{
				Id = 3,
				AccountId = 999,
				MeterReadingDateTime = new DateTime(2003, 3, 3),
				MeterReadingValue = 3333
			};

			// Act & Assert
			ActionResult<MeterReadingDto> actionResult = await controller.CreateMeterReading(newReading);
			Assert.IsType<BadRequestResult>(actionResult.Result);
		}

		[Fact]
		public async Task Cant_create_meter_reading_with_duplicate_id()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);
			MeterReadingsController controller = new(service);
			MeterReadingDto newReading = new()
			{
				Id = 1,
				AccountId = 1,
				MeterReadingDateTime = new DateTime(2003, 3, 3),
				MeterReadingValue = 3333
			};

			// Act & Assert
			ActionResult<MeterReadingDto> actionResult = await controller.CreateMeterReading(newReading);
			Assert.IsType<BadRequestResult>(actionResult.Result);
		}

		[Fact]
		public async Task Can_get_meter_readings()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);
			MeterReadingsController controller = new(service);

			// Act
			ActionResult<IEnumerable<MeterReadingDto>> actionResult = await controller.GetMeterReadings();
			Assert.IsType<OkObjectResult>(actionResult.Result);
			List<MeterReadingDto> readings = GetObjectResultContent(actionResult).ToList();

			// Assert
			Assert.Equal(2, readings.Count);

			Assert.Equal(1, readings[0].Id);
			Assert.Equal(1, readings[0].AccountId);
			Assert.Equal(new DateTime(2001, 1, 1), readings[0].MeterReadingDateTime);
			Assert.Equal(1111, readings[0].MeterReadingValue);

			Assert.Equal(2, readings[1].Id);
			Assert.Equal(1, readings[1].AccountId);
			Assert.Equal(new DateTime(2002, 2, 2), readings[1].MeterReadingDateTime);
			Assert.Equal(2222, readings[1].MeterReadingValue);
		}

		[Fact]
		public async Task Can_delete_all_meter_readings()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);
			MeterReadingsController controller = new(service);

			// Act
			ActionResult<object> actionResult = await controller.DeleteMeterReadings();
			Assert.IsType<OkObjectResult>(actionResult.Result);
			object response = GetObjectResultContent(actionResult);

			DeleteMeterReadingsResponse reading = JObject.FromObject(response).ToObject<DeleteMeterReadingsResponse>();

			// Assert
			Assert.True(reading.Deleted);
		}

		private class DeleteMeterReadingsResponse
		{
			[JsonProperty("deleted")]
			public bool Deleted { get; set; }
		}
	}
}