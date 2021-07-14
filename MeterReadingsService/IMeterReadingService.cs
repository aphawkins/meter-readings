namespace MeterReadingsService
{
	using System.IO;
	using System.Threading.Tasks;
	using MeterReadingsService.Dto;

	public interface IMeterReadingService : IRepository<MeterReadingDto>
	{
		public Task<(int total, int successful)> AddMeterReadingsAsync(StreamReader csvFile);
	}
}