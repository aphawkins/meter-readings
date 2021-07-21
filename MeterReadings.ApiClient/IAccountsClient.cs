namespace MeterReadings.ApiClient
{
	using System.Threading.Tasks;
	using MeterReadings.Dto;

	public interface IAccountsClient
	{
		Task<AccountDto> CreateAccount(AccountDto account);

		Task DeleteAccount(int id);

		Task<AccountDto> GetAccount(int id);

		Task<AccountDto[]> GetAllAccounts();

		Task<AccountDto> UpdateAccount(AccountDto account);
	}
}