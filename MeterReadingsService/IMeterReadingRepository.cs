namespace MeterReadingsService
{
	using System.IO;
	using System.Threading.Tasks;
	using MeterReadingsDto;

	public interface IMeterReadingRepository : IRepository<MeterReadingDto>
	{
		public Task<(int total, int successful)> AddMeterReadingsAsync(StreamReader csvFile);
	}
}