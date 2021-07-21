namespace MeterReadings.ApiClient
{
	public interface IMeterReadingsApiClient
	{
		IAccountsClient Accounts { get; }

		IMeterReadingsClient MeterReadings { get; }
	}
}
