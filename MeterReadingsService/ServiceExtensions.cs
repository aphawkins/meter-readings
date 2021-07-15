namespace MeterReadingsService
{
	using MeterReadingsData;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;

	public static class ServiceExtensions
	{
		public static void ConfigureInMemoryDbAndSeed(this IServiceCollection services, string connectionString)
		{
			// Note: this doesn't support referential integrity
			services.AddDbContext<MainDbContext>(opt => opt.UseInMemoryDatabase(connectionString));

			DataGenerator.Seed(new DbContextOptionsBuilder().UseInMemoryDatabase(connectionString));
		}

		public static void ConfigureSqlServerDb(this IServiceCollection services, string connectionString)
		{
			// To ensure referential integrity use a SQL Server DB.
			services.AddDbContext<MainDbContext>(opt => opt.UseSqlServer(connectionString));

			// PM> Add-Migration initial
			// PM> Update-Database
		}

		public static void ConfigureMeterReadingsService(this IServiceCollection services)
		{
			services.AddScoped<IMeterReadingsService, MeterReadingsService>();
		}
	}
}
