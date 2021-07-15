﻿namespace MeterReadingsServiceTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using MeterReadingsData;
	using MeterReadingsService;
	using MeterReadingsService.Dto;
	using Microsoft.EntityFrameworkCore;
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
			MeterReadingDto newReading = new()
			{
				Id = 3,
				AccountId = 1,
				MeterReadingDateTime = new DateTime(2003, 3, 3),
				MeterReadingValue = 3333,
			};

			// Act
			MeterReadingDto reading = service.MeterReading.Create(newReading);

			// Assert
			Assert.Equal(3, reading.Id);
			Assert.Equal(1, reading.AccountId);
			Assert.Equal(new DateTime(2003, 3, 3), reading.MeterReadingDateTime);
			Assert.Equal(3333, reading.MeterReadingValue);

			Assert.Equal(3, (await service.MeterReading.ReadAsync()).Count());
		}

		[Fact]
		public async Task Can_read_all_meter_readings()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);

			// Act
			List<MeterReadingDto> readings = (await service.MeterReading.ReadAsync()).ToList();

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
		public async Task Can_read_meter_reading_by_id()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);

			// Act
			List<MeterReadingDto> readings = (await service.MeterReading.ReadAsync(e => e.Id == 1)).ToList();

			// Assert
			Assert.Single(readings);
			Assert.Equal(1, readings[0].Id);
			Assert.Equal(1, readings[0].AccountId);
			Assert.Equal(new DateTime(2001, 1, 1), readings[0].MeterReadingDateTime);
			Assert.Equal(1111, readings[0].MeterReadingValue);
		}


		[Fact]
		public async Task Can_update_meter_reading()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);
			MeterReadingDto existing = (await service.MeterReading.ReadAsync(x => x.Id == 1)).FirstOrDefault();
			existing.MeterReadingDateTime = new DateTime(2011, 11, 11);
			existing.MeterReadingValue = 11111;

			// Act
			MeterReadingDto reading = service.MeterReading.Update(existing);

			// Assert
			Assert.Equal(1, reading.Id);
			Assert.Equal(1, reading.AccountId);
			Assert.Equal(new DateTime(2011, 11, 11), reading.MeterReadingDateTime);
			Assert.Equal(11111, reading.MeterReadingValue);
		}

		[Fact]
		public async Task Can_delete_meter_reading_by_id()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);
			MeterReadingDto reading = (await service.MeterReading.ReadAsync(x => x.Id == 1)).FirstOrDefault();

			// Act
			service.MeterReading.Delete(reading);

			// Assert
			Assert.Empty(await service.MeterReading.ReadAsync(x => x.Id == 1));
			Assert.Single(await service.MeterReading.ReadAsync());
		}

		[Fact]
		public async Task Cant_delete_no_meter_reading_by_id()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);

			// Act
			service.MeterReading.Delete(null);

			// Assert
			Assert.Equal(2, (await service.MeterReading.ReadAsync()).Count());
		}

		[Fact]
		public async Task Can_delete_all_meter_readings()
		{
			// Arrange
			using MainDbContext context = new(ContextOptions);
			IMeterReadingsService service = new MeterReadingsService(context);

			// Act
			service.MeterReading.Delete();

			// Assert
			Assert.Empty(await service.MeterReading.ReadAsync());
		}
	}
}