namespace MeterReadingsData
{
	using MeterReadingsData.Models;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;
	using System;
	using System.IO;
	using System.Linq;
	using System.Reflection;

	public static class DataGenerator
    {
		public static void Initialize(IServiceProvider serviceProvider)
		{
			using var context = new MainDbContext(serviceProvider.GetRequiredService<DbContextOptions<MainDbContext>>());

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
                            new Account
                            {
                                Id = accountId,
                                FirstName = details[1],
                                LastName = details[2],
                            });
                    }
                }

                file.Close();
            }

            context.SaveChanges();
        }
    }
}