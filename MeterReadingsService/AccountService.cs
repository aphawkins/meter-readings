namespace MeterReadingsService
{
	using MeterReadings.DTO;
	using MeterReadingsData;
	using MeterReadingsData.Models;
	using System.Linq;

	public class AccountService : IAccountService
    {
		private readonly MainDbContext _context;

		private IQueryable<AccountDto> MapAccountToDto(IQueryable<Account> accounts)
		{
			return accounts.Select(acc => new AccountDto
			{

				AccountId = acc.Id,
				FirstName = acc.FirstName,
				LastName = acc.LastName,
			});
		}



		public AccountService(MainDbContext context)
		{
			_context = context;
		}

		public IQueryable<AccountDto> GetAllAccounts()
		{
			return MapAccountToDto(_context.Accounts);
		}

		public IQueryable<AccountDto> GetAccountById(int accountId)
		{
			return MapAccountToDto(_context.Accounts.Where(acc => accountId == acc.Id));
		}

		public Account UpdateBook(AccountDto dto)
		{
			Account account = _context.Accounts.Find(dto.AccountId);
			account.FirstName = dto.FirstName;
			account.LastName = dto.LastName;
			_context.SaveChanges();
			return account;
		}
	}
}
