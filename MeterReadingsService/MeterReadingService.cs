namespace MeterReadingsService
{
	using MeterReadings.DTO;
	using MeterReadingsData;
	using MeterReadingsData.Models;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.ChangeTracking;
	using System;
	using System.Linq;
	using System.Threading.Tasks;

	public class MeterReadingService : IMeterReadingService
	{
		private readonly MainDbContext _context;

		public MeterReadingService(MainDbContext context)
		{
			_context = context;
		}

		public IQueryable<MeterReadingDto> GetAllMeterReadings()
		{
			return MapMeterReadingToDto(_context.MeterReadings);
		}

		public async Task<MeterReadingDto> GetMeterReadingAsync(int accountId, DateTime meterReadingDateTime)
		{
			MeterReading reading = await _context.MeterReadings.FirstOrDefaultAsync(x => x.AccountId == accountId && x.MeterReadingDateTime == meterReadingDateTime);
			if (reading == null)
			{
				return null;
			}

			return MapMeterReadingToDto(reading);
		}

		public async Task<MeterReadingDto> AddMeterReadingAsync(int accountId, DateTime meterReadingDateTime, int meterReadingValue)
		{
			EntityEntry<MeterReading> reading = await _context.MeterReadings.AddAsync(
				new MeterReading
				{
					AccountId = accountId,
					MeterReadingDateTime = meterReadingDateTime,
					MeterReadingValue = meterReadingValue,
				});

			int count = await _context.SaveChangesAsync();
			if (count < 1)
			{
				return null;
			}

			return MapMeterReadingToDto(reading.Entity);
		}

		public async Task<int> DeleteAllMeterReadingsAsync()
		{
			foreach (MeterReading entity in _context.MeterReadings)
			{
				_context.MeterReadings.Remove(entity);
			}

			return await _context.SaveChangesAsync();
		}

		private static MeterReadingDto MapMeterReadingToDto(MeterReading reading)
		{
			return new MeterReadingDto
			{
				AccountId = reading.AccountId,
				MeterReadingDateTime = reading.MeterReadingDateTime,
				MeterReadValue = reading.MeterReadingValue,
			};
		}

		private static IQueryable<MeterReadingDto> MapMeterReadingToDto(IQueryable<MeterReading> readings)
		{
			return readings.Select(meterReading => MapMeterReadingToDto(meterReading));
		}
	}
}
