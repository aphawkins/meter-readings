namespace MeterReadingsService
{
	using System.Linq;
	using System.Threading.Tasks;
	using MeterReadings.DTO;
	using MeterReadingsData;
	using MeterReadingsData.Models;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.ChangeTracking;

	public class AccountService : IAccountService
    {
		private readonly MainDbContext _context;

		public AccountService(MainDbContext context)
		{
			_context = context;
		}

		public async Task<AccountDto> CreateAsync(AccountDto item)
		{
			Account newAccount = new()
			{
				Id = item.Id,
				FirstName = item.FirstName,
				LastName = item.LastName
			};

			EntityEntry<Account> addedAccount = await _context.Accounts.AddAsync(newAccount);
			if (addedAccount == null)
			{
				return null;
			}

			await _context.SaveChangesAsync();

			return MapAccountToDto(addedAccount.Entity);
		}

		public IQueryable<AccountDto> Read()
		{
			return MapAccountToDto(_context.Accounts);
		}

		public async Task<AccountDto> ReadAsync(int id)
		{
			Account account = await _context.Accounts.FindAsync(id);
			if (account == null)
			{
				return null;
			}	

			return MapAccountToDto(account);
		}

		public async Task<AccountDto> UpdateAsync(AccountDto item)
		{
			Account account = _context.Accounts.Find(item.Id);

			try
			{
				account.FirstName = item.FirstName;
				account.LastName = item.LastName;
				int numChanges = await _context.SaveChangesAsync();
				if (numChanges < 1)
				{
					return null;
				}
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!AccountExists(account.Id))
				{
					return null;
				}
			}

			return MapAccountToDto(account);
		}

		public async Task<int> DeleteAsync()
		{
			foreach (Account entity in _context.Accounts)
			{
				_context.Accounts.Remove(entity);
			}

			return await _context.SaveChangesAsync();
		}

		public async Task<bool> DeleteAsync(int id)
		{
			Account account = await _context.Accounts.FindAsync(id);
			_context.Accounts.Remove(account);
			return await _context.SaveChangesAsync() > 0;
		}

		private bool AccountExists(int id)
		{
			return _context.Accounts.Any(e => e.Id == id);
		}

		private static AccountDto MapAccountToDto(Account account)
		{
			return new AccountDto
			{
				Id = account.Id,
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
