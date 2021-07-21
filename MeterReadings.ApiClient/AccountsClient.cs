namespace MeterReadings.ApiClient
{
	using System.Net.Http;
	using System.Net.Http.Json;
	using System.Threading.Tasks;
	using MeterReadings.Dto;

	public class AccountsClient : IAccountsClient
	{
		private const string url = "http://localhost:41943/api/accounts";

		private readonly HttpClient _httpClient;

		public AccountsClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<AccountDto> CreateAccount(AccountDto account)
		{
			HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, account);
			return await response.Content.ReadFromJsonAsync<AccountDto>();
		}

		public async Task<AccountDto[]> GetAllAccounts()
		{
			return await _httpClient.GetFromJsonAsync<AccountDto[]>(url);
		}

		public async Task<AccountDto> GetAccount(int id)
		{
			return await _httpClient.GetFromJsonAsync<AccountDto>(url + "/" + id);
		}

		public async Task<AccountDto> UpdateAccount(AccountDto account)
		{
			HttpResponseMessage response = await _httpClient.PutAsJsonAsync(url, account);
			return await response.Content.ReadFromJsonAsync<AccountDto>();
		}

		public async Task DeleteAccount(int id)
		{
			await _httpClient.DeleteAsync(url + "/" + id);
		}
	}
}
