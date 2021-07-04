namespace MeterReadingsApi
{
	using MeterReadingsData;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.OpenApi.Models;

	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// For simplicity use an InMemoryDatabase
			// Note: this doesn't support referential integrity
			services.AddDbContext<MainDbContext>(opt => opt.UseInMemoryDatabase("MainDb"));

			// To ensure referential integrity use a SQL Server DB.
			// Comment out the InMemoryDatabase and uncomment out the next line, then follow the steps in Package Manager Console.
			// PM> Add-Migration initial
			// PM> Update-Database
			//// services.AddDbContext<MainDbContext>(opt => opt.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=meterreadings;Trusted_Connection=True;MultipleActiveResultSets=true"));

			services.AddControllers();
			services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "MeterReadings", Version = "v1" }));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MeterReadings v1"));
			}

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints => endpoints.MapControllers());
		}
	}
}