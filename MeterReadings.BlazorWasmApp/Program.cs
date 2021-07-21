namespace MeterReadings.BlazorWasmApp
{
	using System;
	using System.Net.Http;
	using System.Threading.Tasks;
	using MeterReadings.ApiClient;
	using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
	using Microsoft.Extensions.DependencyInjection;

	public static class Program
    {
        public static async Task Main(string[] args)
        {
			WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:41943") });
			builder.Services.AddScoped<IMeterReadingsApiClient, MeterReadingsApiClient>();

			await builder.Build().RunAsync();
        }
    }
}
