namespace MeterReadingsApiTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using MeterReadingsApi.Controllers;
	using MeterReadingsData;
	using MeterReadingsService;
	using MeterReadingsService.Dto;
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
		public void Can_get_meter_readings()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);
			MeterReadingsController controller = new(service);

			// Act
			ActionResult<IQueryable<MeterReadingDto>> actionResult = controller.GetMeterReadings();
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
		public void Can_delete_all_meter_readings()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);
			MeterReadingsController controller = new(service);

			// Act
			ActionResult<object> actionResult = controller.DeleteMeterReadings();
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