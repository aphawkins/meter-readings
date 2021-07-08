namespace MeterReadingsService
{
	using MeterReadings.DTO;
	using MeterReadingsData;
	using MeterReadingsData.Models;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.ChangeTracking;
	using System;
	using System.IO;
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


		public async Task<(int total, int successful)> AddMeterReadingsAsync(StreamReader csvFile)
		{
			int successful = 0;
			int total = 0;
			string line;
			IAccountService accountService = new AccountService(_context);

			// Assume header on first line
			await csvFile.ReadLineAsync();

			while ((line = await csvFile.ReadLineAsync()) != null)
			{
				string[] details = line.Split(',');

				if (details.Length >= 3 &&
					int.TryParse(details[0], out int accountId) &&
					DateTime.TryParse(details[1], out DateTime readingDT) &&
					int.TryParse(details[2], out int readingValue) &&
					readingValue >= 0 &&
					readingValue < 100000)
				{
					// Does account exist?
					AccountDto account = await accountService.GetAccountAsync(accountId);

					// Does reading already exist?
					MeterReadingDto reading = await GetMeterReadingAsync(accountId, readingDT);

					if (account != null &&
						reading == null)
					{
						MeterReadingDto newReading = await AddMeterReadingAsync(accountId, readingDT, readingValue);
						if (newReading != null)
						{
							successful++;
						}
					}
				}

				total++;
			}

			return ( total, successful );
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
