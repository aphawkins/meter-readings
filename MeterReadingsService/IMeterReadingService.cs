namespace MeterReadingsService
{
	using MeterReadings.DTO;
	using System;
	using System.Linq;
	using System.Threading.Tasks;

	public interface IMeterReadingService
	{
		public IQueryable<MeterReadingDto> GetAllMeterReadings();

		public Task<MeterReadingDto> GetMeterReadingAsync(int accountId, DateTime meterReadingDateTime);

		public Task<MeterReadingDto> AddMeterReadingAsync(int accountId, DateTime meterReadingDateTime, int meterReadingValue);

		public Task<int> DeleteAllMeterReadingsAsync();
	}
}