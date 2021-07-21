namespace MeterReadings.ApiClient
{
	using System.Net.Http;
	using System.Net.Http.Json;
	using System.Threading.Tasks;
	using MeterReadings.Dto;

	public class MeterReadingsClient : IMeterReadingsClient
	{
		private readonly HttpClient _httpClient;

		public MeterReadingsClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<MeterReadingDto> CreateAsync(MeterReadingDto item)
		{
			HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/meterreadings", item);
			return await response.Content.ReadFromJsonAsync<MeterReadingDto>();
		}

		public async Task<MeterReadingDto> GetAsync(int id)
		{
			return await _httpClient.GetFromJsonAsync<MeterReadingDto>("/api/meterreadings/" + id);
		}

		public async Task<MeterReadingDto[]> GetAllAsync()
		{
			return await _httpClient.GetFromJsonAsync<MeterReadingDto[]>("/api/meterreadings");
		}

		public async Task<MeterReadingDto> UpdateAsync(MeterReadingDto item)
		{
			HttpResponseMessage response = await _httpClient.PutAsJsonAsync("/api/meterreadings", item);
			return await response.Content.ReadFromJsonAsync<MeterReadingDto>();
		}

		public async Task DeleteAsync(int id)
		{
			await _httpClient.DeleteAsync("/api/meterreadings/" + id);
		}
	}
}
