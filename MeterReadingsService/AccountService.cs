namespace MeterReadingsService
{
	using MeterReadings.DTO;
	using MeterReadingsData;
	using MeterReadingsData.Models;
	using System;
	using System.Linq;
	using System.Threading.Tasks;

	public class AccountService : IAccountService
    {
		private readonly MainDbContext _context;

		public AccountService(MainDbContext context)
		{
			_context = context;
		}

		public IQueryable<AccountDto> GetAllAccounts()
		{
			return MapAccountToDto(_context.Accounts);
		}

		public async Task<AccountDto> GetAccountAsync(int accountId)
		{
			var account = await _context.Accounts.FindAsync(accountId);
			if (account == null)
			{
				return null;
			}	

			return MapAccountToDto(account);
		}

		public Account UpdateAccount(AccountDto dto)
		{
			Account account = _context.Accounts.Find(dto.AccountId);
			account.FirstName = dto.FirstName;
			account.LastName = dto.LastName;
			_context.SaveChanges();
			return account;
		}

		private static AccountDto MapAccountToDto(Account account)
		{
			return new AccountDto
			{
				AccountId = account.Id,
				FirstName = account.FirstName,
				LastName = account.LastName,
			};
		}

		private static IQueryable<AccountDto> MapAccountToDto(IQueryable<Account> accounts)
		{
			return accounts.Select(account => MapAccountToDto(account));
		}
	}
}
