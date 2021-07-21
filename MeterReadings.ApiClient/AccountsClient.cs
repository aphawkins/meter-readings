namespace MeterReadings.ApiClient
{
	using System.Net.Http;
	using System.Net.Http.Json;
	using System.Threading.Tasks;
	using MeterReadings.Dto;

	public class AccountsClient : IAccountsClient
	{
		private readonly HttpClient _httpClient;

		public AccountsClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<AccountDto> CreateAsync(AccountDto item)
		{
			HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/accounts", item);
			return await response.Content.ReadFromJsonAsync<AccountDto>();
		}

		public async Task<AccountDto> GetAsync(int id)
		{
			return await _httpClient.GetFromJsonAsync<AccountDto>("/api/accounts/" + id);
		}

		public async Task<AccountDto[]> GetAllAsync()
		{
			return await _httpClient.GetFromJsonAsync<AccountDto[]>("/api/accounts");
		}

		public async Task<AccountDto> UpdateAsync(AccountDto item)
		{
			HttpResponseMessage response = await _httpClient.PutAsJsonAsync("/api/accounts", item);
			return await response.Content.ReadFromJsonAsync<AccountDto>();
		}

		public async Task DeleteAsync(int id)
		{
			await _httpClient.DeleteAsync("/api/accounts/" + id);
		}
	}
}
