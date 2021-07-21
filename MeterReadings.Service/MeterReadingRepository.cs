namespace MeterReadings.Service
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	using MeterReadings.DB;
	using MeterReadings.DB.Entities;
	using MeterReadings.Dto;

	public class MeterReadingRepository : RepositoryBase<MeterReadingDto, MeterReading>, IMeterReadingRepository
	{
		public MeterReadingRepository(MainDbContext repositoryContext) : base(repositoryContext)
		{
		}

		public async Task<(int total, int successful)> AddMeterReadingsAsync(StreamReader csvFile)
		{
			int successful = 0;
			int total = 0;
			string line;

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
					readingValue < 100000 &&
					RepositoryContext.Accounts.Any(x => x.Id == accountId) &&
					!RepositoryContext.MeterReadings.Any(x => x.AccountId == accountId && x.MeterReadingDateTime == readingDT))
				{
					MeterReadingDto newReading = await CreateAsync(new()
					{
						AccountId = accountId,
						MeterReadingDateTime = readingDT,
						MeterReadingValue = readingValue,
					});

					if (newReading != null)
					{
						successful++;
					}
				}

				total++;
			}

			return (total, successful);
		}

	}
}