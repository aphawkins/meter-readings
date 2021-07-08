namespace MeterReadingsService
{
	using MeterReadings.DTO;
	using MeterReadingsData.Models;
	using System.Linq;

	public static class MeterReadingService
    {
		public static IQueryable<MeterReadingDto> MapMeterReadingToDto(this IQueryable<MeterReading> reading)
		{
			return reading.Select(mr => new MeterReadingDto
			{
				AccountId = mr.AccountId,
				MeterReadingDateTime = mr.MeterReadingDateTime,
				MeterReadValue = mr.MeterReadingValue,
			});
		}
	}
}
