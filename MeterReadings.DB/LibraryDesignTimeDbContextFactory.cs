namespace MeterReadings.DB
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Design;

	public class LibraryDesignTimeDbContextFactory : IDesignTimeDbContextFactory<MainDbContext>
    {
        private const string connectionString = @"Server=(localdb)\mssqllocaldb;Database=meterreadings;Trusted_Connection=True;MultipleActiveResultSets=true";

        public MainDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MainDbContext>();

            builder.UseSqlServer(connectionString);
            return new MainDbContext(builder.Options);
        }
    }
}