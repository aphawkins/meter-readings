namespace MeterReadingsService
{
	using MeterReadings.DTO;
	using MeterReadingsData.Models;
	using System.Linq;

	public static class AccountService
    {
		public static IQueryable<AccountDto> MapAccountToDto(this IQueryable<Account> accounts)
		{
			return accounts.Select(acc => new AccountDto
			{

				AccountId = acc.Id,
				FirstName = acc.FirstName,
				LastName = acc.LastName,
			});
		}
	}
}
