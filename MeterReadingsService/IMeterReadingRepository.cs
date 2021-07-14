namespace MeterReadingsService
{
	using System.IO;
	using System.Threading.Tasks;
	using global::MeterReadingsService.Dto;

	public interface IMeterReadingRepository : IRepository<MeterReadingDto>
	{
		public Task<(int total, int successful)> AddMeterReadingsAsync(StreamReader csvFile);
	}
}