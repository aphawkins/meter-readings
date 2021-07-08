namespace MeterReadingsService
{
	using MeterReadings.DTO;
	using System;
	using System.Linq;
	using System.Threading.Tasks;

	public interface IAccountService
	{
		public IQueryable<AccountDto> GetAllAccounts();

		public Task<AccountDto> GetAccountAsync(int accountId);
	}
}