namespace MeterReadings.ApiClient
{
	using System.Threading.Tasks;

	public interface IClient<T>
	{
		Task<T> CreateAsync(T item);

		Task<T> GetAsync(int id);

		Task<T[]> GetAllAsync();

		Task<T> UpdateAsync(T item);

		Task DeleteAsync(int id);
	}
}