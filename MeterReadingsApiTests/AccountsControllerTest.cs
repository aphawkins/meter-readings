namespace MeterReadingsApiTests
{
	using MeterReadingsData;
	using MeterReadingsData.Models;
	using Microsoft.EntityFrameworkCore;

	public class AccountsControllerTest
	{
		protected AccountsControllerTest(DbContextOptions<MainDbContext> contextOptions)
		{
			ContextOptions = contextOptions;

			Seed();
		}

		protected DbContextOptions<MainDbContext> ContextOptions { get; }

		private void Seed()
		{
			using MainDbContext context = new(ContextOptions);
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			Account one = new()
			{
				Id = 1,
				FirstName = "One",
				LastName = "World"
			};

			Account two = new()
			{
				Id = 2,
				FirstName = "Two",
				LastName = "Second"
			};

			context.AddRange(one, two);

			context.SaveChanges();
		}
	}
}