namespace MeterReadings.TestLibrary
{
	using System;
	using MeterReadings.Data;
	using MeterReadings.Data.Entities;
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
				FirstName = "One",
				LastName = "First"
			});

			context.Add(new Account()
			{
				FirstName = "Two",
				LastName = "Second"
			});

			context.Add(new MeterReading()
			{
				AccountId = 1,
				MeterReadingDateTime = new DateTime(2001, 1, 1),
				MeterReadingValue = 1111,
			});

			context.Add(new MeterReading()
			{
				AccountId = 1,
				MeterReadingDateTime = new DateTime(2002, 2, 2),
				MeterReadingValue = 2222,
			});
			
			context.SaveChanges();
		}
	}
}