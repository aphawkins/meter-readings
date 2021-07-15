namespace MeterReadingsMvcApp
{
	using AutoMapper;
	using MeterReadingsService.Dto;
	using MeterReadingsMvcApp.Models;

	internal static class MapperConfig
	{
		internal readonly static MapperConfiguration Config = new(cfg => 
		{ 
			cfg.CreateMap<AccountViewModel, AccountDto>(); 
			cfg.CreateMap<AccountDto, AccountViewModel>();
			cfg.CreateMap<MeterReadingViewModel, MeterReadingDto>(); 
			cfg.CreateMap<MeterReadingDto, MeterReadingViewModel>();
		});
	}
}
