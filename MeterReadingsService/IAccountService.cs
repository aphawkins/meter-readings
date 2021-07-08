namespace MeterReadingsService
{
	using MeterReadings.DTO;
	using System.Linq;

	public interface IAccountService
	{
		public IQueryable<AccountDto> GetAllAccounts();

		public IQueryable<AccountDto> GetAccountById(int accountId);
	}
}