namespace MeterReadingsService
{
	using MeterReadingsData;
	using MeterReadingsData.Entities;
	using global::MeterReadingsService.Dto;

	public class AccountRepository : RepositoryBase<AccountDto, Account>, IAccountRepository
	{
		public AccountRepository(MainDbContext repositoryContext) : base(repositoryContext)
		{
		}
	}
}