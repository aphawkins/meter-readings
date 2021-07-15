namespace MeterReadingsApiTests
{
	using System;
	using MeterReadingsData;
	using MeterReadingsData.Entities;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;

	public class ControllerTestsBase
	{
		protected ControllerTestsBase(DbContextOptions<MainDbContext> contextOptions)
		{
			ContextOptions = contextOptions;

			Seed();
		}

		protected DbContextOptions<MainDbContext> ContextOptions { get; }

		protected static T GetObjectResultContent<T>(ActionResult<T> result)
		{
			return (T)((ObjectResult)result.Result).Value;
		}

		private void Seed()
		{
			using MainDbContext context = new(ContextOptions);
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			context.Add(new Account()
			{
				Id = 1,
				FirstName = "One",
				LastName = "First"
			});

			context.Add(new Account()
			{
				Id = 2,
				FirstName = "Two",
				LastName = "Second"
			});

			context.Add(new MeterReading()
			{
				Id = 1,
				AccountId = 1,
				MeterReadingDateTime = new DateTime(2001, 1, 1),
				MeterReadingValue = 1111,
			});

			context.Add(new MeterReading()
			{
				Id = 2,
				AccountId = 1,
				MeterReadingDateTime = new DateTime(2002, 2, 2),
				MeterReadingValue = 2222,
			});
			
			context.SaveChanges();
		}
	}
}