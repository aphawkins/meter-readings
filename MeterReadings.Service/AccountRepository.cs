namespace MeterReadings.Service
{
	using MeterReadings.DB;
	using MeterReadings.DB.Entities;
	using MeterReadings.Dto;

	public class AccountRepository : RepositoryBase<AccountDto, Account>, IAccountRepository
	{
		public AccountRepository(MainDbContext repositoryContext) : base(repositoryContext)
		{
		}
	}
}