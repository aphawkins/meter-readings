namespace MeterReadings.Data
{
	using Microsoft.EntityFrameworkCore;
	using System;
	using System.IO;
	using System.Reflection;

	public static class DataGenerator
	{
		public static void Seed(DbContextOptionsBuilder contextOptions)
		{
			using MainDbContext context = new(contextOptions.Options);
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			// Read the test accounts file.
			string line;
			using (StreamReader file = new(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Test_Accounts.csv")))
			{
				while ((line = file.ReadLine()) != null)
				{
					string[] details = line.Split(',');

					if (int.TryParse(details[0], out int accountId))
					{
						context.Accounts.Add(
							new()
							{
								Id = accountId,
								FirstName = details[1],
								LastName = details[2],
							});
					}
				}

				file.Close();
			}

			context.MeterReadings.Add(new() { AccountId = 2344, MeterReadingDateTime = new DateTime(2001, 1, 1, 1, 1, 1, 1), MeterReadingValue = 11111 });
			context.MeterReadings.Add(new() { AccountId = 2233, MeterReadingDateTime = new DateTime(2002, 2, 2, 2, 2, 2, 2), MeterReadingValue = 22222 });
			context.MeterReadings.Add(new() { AccountId = 8766, MeterReadingDateTime = DateTime.UtcNow, MeterReadingValue = 12345 });

			context.SaveChanges();
		}
	}
}