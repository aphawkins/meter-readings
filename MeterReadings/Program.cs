namespace MeterReadingsApi
{
	using System.Reflection;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Hosting;

	public static class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			string assemblyName = typeof(Startup).GetTypeInfo().Assembly.FullName;

			return Host
				.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup(assemblyName));
		}
	}
}