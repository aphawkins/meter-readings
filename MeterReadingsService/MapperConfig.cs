namespace MeterReadingsService
{
	using AutoMapper;
	using global::MeterReadingsService.Dto;
	using MeterReadingsData.Entities;

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
