namespace MeterReadingsService
{
	using MeterReadings.DTO;
	using System;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;

	public interface IMeterReadingService : IRepository<MeterReadingDto>
	{
		public Task<(int total, int successful)> AddMeterReadingsAsync(StreamReader csvFile);
	}
}