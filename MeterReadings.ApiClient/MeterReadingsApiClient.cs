namespace MeterReadings.ApiClient
{
	using System.Net.Http;

	public class MeterReadingsApiClient : IMeterReadingsApiClient
	{
		public MeterReadingsApiClient(HttpClient httpClient)
		{
			Accounts = new AccountsClient(httpClient);
			MeterReadings = new MeterReadingsClient(httpClient);
		}

		public  IAccountsClient Accounts { get; }

		public IMeterReadingsClient MeterReadings { get; }
	}
}
