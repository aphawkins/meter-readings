namespace MeterReadingsService
{
	using MeterReadingsData;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;

	public static class ServiceExtensions
	{
		public static void ConfigureRepositoryWrapper(this IServiceCollection services)
		{
			// For simplicity use an InMemoryDatabase
			// Note: this doesn't support referential integrity
			services.AddDbContext<MainDbContext>(opt => opt.UseInMemoryDatabase("MainDb"));

			// To ensure referential integrity use a SQL Server DB.
			// Comment out the InMemoryDatabase and uncomment out the next line, then follow the steps in Package Manager Console.
			// PM> Add-Migration initial
			// PM> Update-Database
			//// services.AddDbContext<MainDbContext>(opt => opt.UseSqlServer("MeterReadingsDatabase"));

			services.AddScoped<IMeterReadingsService, MeterReadingsService>();
		}
	}
}
