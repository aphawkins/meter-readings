namespace MeterReadingsService
{
	public interface IMeterReadingsService
	{
		IAccountRepository Account { get; }

		IMeterReadingRepository MeterReading { get; }

		void Save();
	}
}
