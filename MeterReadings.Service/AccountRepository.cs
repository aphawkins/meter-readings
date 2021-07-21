namespace MeterReadings.Service
{
	using MeterReadings.Data;
	using MeterReadings.Data.Entities;
	using MeterReadings.Dto;

	public class AccountRepository : RepositoryBase<AccountDto, Account>, IAccountRepository
	{
		public AccountRepository(MainDbContext repositoryContext) : base(repositoryContext)
		{
		}
	}
}