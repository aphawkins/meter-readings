namespace MeterReadingsService
{
	using AutoMapper;
	using MeterReadingsData.Entities;
	using MeterReadingsDto;

	public static class DtoMapperConfig
	{
		public readonly static MapperConfiguration Config = new(cfg => 
		{ 
			cfg.CreateMap<Account, AccountDto>(); 
			cfg.CreateMap<AccountDto, Account>();
			cfg.CreateMap<MeterReading, MeterReadingDto>(); 
			cfg.CreateMap<MeterReadingDto, MeterReading>();
		});
	}
}
