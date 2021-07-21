namespace MeterReadings.Service
{
	using System.IO;
	using System.Threading.Tasks;
	using MeterReadings.Dto;

	public interface IMeterReadingRepository : IRepository<MeterReadingDto>
	{
		public Task<(int total, int successful)> AddMeterReadingsAsync(StreamReader csvFile);
	}
}