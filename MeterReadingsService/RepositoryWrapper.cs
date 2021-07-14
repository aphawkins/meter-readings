namespace MeterReadingsService
{
	using MeterReadingsData;

	public class MeterReadingsService : IMeterReadingsService
	{
		private readonly MainDbContext _repoContext;

		private IAccountRepository _account;

		private IMeterReadingRepository _meterReading;

		public MeterReadingsService(MainDbContext repositoryContext)
		{
			_repoContext = repositoryContext;
		}

		public IAccountRepository Account
		{
			get
			{
				if (_account == null)
				{
					_account = new AccountRepository(_repoContext);
				}
				return _account;
			}
		}

		public IMeterReadingRepository MeterReading
		{
			get
			{
				if (_meterReading == null)
				{
					_meterReading = new MeterReadingRepository(_repoContext);
				}
				return _meterReading;
			}
		}

		public void Save()
		{
			_repoContext.SaveChanges();
		}
	}
}
