namespace MeterReadings.Service
{
	using AutoMapper;
	using MeterReadings.Data.Entities;
	using MeterReadings.Dto;

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
