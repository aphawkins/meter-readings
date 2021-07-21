namespace MeterReadings.Api
{
	using MeterReadings.Service;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.OpenApi.Models;

	public class StartupDevelopment
	{
		public StartupDevelopment(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.ConfigureInMemoryDbAndSeed(Configuration.GetConnectionString("MeterReadingsDatabase"));
			services.ConfigureMeterReadingsService();

			services.AddControllers();
			services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "MeterReadings", Version = "v1" }));

			services.AddCors(options => options.AddDefaultPolicy(builder =>
				builder.WithOrigins("https://localhost:44315")
					   .AllowAnyMethod()
					   .AllowAnyHeader()));
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
			app.UseCors();
			app.UseAuthorization();

			app.UseEndpoints(endpoints => endpoints.MapControllers());
		}
	}
}