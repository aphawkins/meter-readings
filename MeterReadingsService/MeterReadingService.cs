namespace MeterReadingsService
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	using MeterReadings.DTO;
	using MeterReadingsData;
	using MeterReadingsData.Models;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.ChangeTracking;

	public class MeterReadingService : IMeterReadingService
	{
		private readonly MainDbContext _context;

		public MeterReadingService(MainDbContext context)
		{
			_context = context;
		}

		public async Task<MeterReadingDto> CreateAsync(MeterReadingDto item)
		{
			EntityEntry<MeterReading> reading = await _context.MeterReadings.AddAsync(MapDtoToMeterReading(item));

			int count = await _context.SaveChangesAsync();
			if (count < 1)
			{
				return null;
			}

			return MapMeterReadingToDto(reading.Entity);
		}

		public IQueryable<MeterReadingDto> Read()
		{
			var temp = _context.MeterReadings.Include(m => m.MyAccount);
			return MapMeterReadingToDto(temp);
		}

		public async Task<MeterReadingDto> ReadAsync(int id)
		{
			MeterReading meterReading = await _context.MeterReadings
				.Include(m => m.MyAccount)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (meterReading == null)
			{
				return null;
			}

			return MapMeterReadingToDto(meterReading);
		}

		private async Task<MeterReadingDto> ReadAsync(int accountId, DateTime meterReadingDateTime)
		{
			MeterReading meterReading = await _context.MeterReadings
				.Include(m => m.MyAccount)
				.FirstOrDefaultAsync(m => m.AccountId == accountId && m.MeterReadingDateTime == meterReadingDateTime);

			if (meterReading == null)
			{
				return null;
			}

			return MapMeterReadingToDto(meterReading);
		}

		public async Task<MeterReadingDto> UpdateAsync(MeterReadingDto item)
		{
			MeterReading reading = _context.MeterReadings.Find(item.Id);

			try
			{
				reading.MeterReadingDateTime = item.MeterReadingDateTime;
				reading.MeterReadingValue = item.MeterReadingValue;
				int numChanges = await _context.SaveChangesAsync();
				if (numChanges < 1)
				{
					return null;
				}
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MeterReadingExists(reading.Id))
				{
					return null;
				}
			}


			return MapMeterReadingToDto(reading);
		}

		public async Task<int> DeleteAsync()
		{
			foreach (MeterReading entity in _context.MeterReadings)
			{
				_context.MeterReadings.Remove(entity);
			}

			return await _context.SaveChangesAsync();
		}

		public async Task<bool> DeleteAsync(int id)
		{
			MeterReading meterReading = await _context.MeterReadings.FindAsync(id);
			_context.MeterReadings.Remove(meterReading);
			return await _context.SaveChangesAsync() > 0;
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
					AccountDto account = await accountService.ReadAsync(accountId);

					// Does reading already exist?
					MeterReadingDto reading = await ReadAsync(accountId, readingDT);

					if (account != null &&
						reading == null)
					{
						MeterReadingDto newReading = await CreateAsync(new()
						{
							MeterReadingDateTime = readingDT,
							MeterReadingValue = readingValue,
						});

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

		private bool MeterReadingExists(int id)
		{
			return _context.MeterReadings.Any(e => e.Id == id);
		}

		private static MeterReading MapDtoToMeterReading(MeterReadingDto reading)
		{
			return new MeterReading()
			{
				Id = reading.Id,
				AccountId = reading.AccountId,
				MeterReadingDateTime = reading.MeterReadingDateTime,
				MeterReadingValue = reading.MeterReadingValue,
			};
		}

		private static MeterReadingDto MapMeterReadingToDto(MeterReading reading)
		{
			return new MeterReadingDto
			{
				Id = reading.Id,
				AccountId = reading.AccountId,
				MeterReadingDateTime = reading.MeterReadingDateTime,
				MeterReadingValue = reading.MeterReadingValue,
			};
		}

		private static IQueryable<MeterReadingDto> MapMeterReadingToDto(IQueryable<MeterReading> readings)
		{
			return readings.Select(m => MapMeterReadingToDto(m));
		}
	}
}
