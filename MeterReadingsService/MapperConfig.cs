namespace MeterReadingsService
{
	using AutoMapper;
	using MeterReadingsData.Models;
	using MeterReadingsService.Dto;

	internal static class MapperConfig
	{
		internal readonly static MapperConfiguration Config = new(cfg => 
		{ 
			cfg.CreateMap<Account, AccountDto>(); 
			cfg.CreateMap<AccountDto, Account>();
			cfg.CreateMap<MeterReading, MeterReadingDto>(); 
			cfg.CreateMap<MeterReadingDto, MeterReading>();
		});
	}
}
