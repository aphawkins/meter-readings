namespace MeterReadings.ApiClient
{
	using System;
	using System.Net.Http;
	using Polly;
	using Polly.Registry;

	public static class ClientRetryPolicy
	{
		private static readonly int maxRetryAttempts = 3;
		private static readonly TimeSpan pauseBetweenFailures = TimeSpan.FromSeconds(2);

		public static PolicyRegistry Registry
		{
			get
			{
				return new PolicyRegistry()
				{
					{
						"asyncRetry",
						Policy
							.Handle<HttpRequestException>()
							.WaitAndRetryAsync(maxRetryAttempts, i => pauseBetweenFailures)
					}
				};
			}
		}
	}
}
