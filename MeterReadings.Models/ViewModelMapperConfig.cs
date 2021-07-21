namespace MeterReadings.Models
{
	using AutoMapper;
	using MeterReadings.Dto;

	public static class ViewModelMapperConfig
	{
		public readonly static MapperConfiguration Config = new(cfg => 
		{ 
			cfg.CreateMap<AccountViewModel, AccountDto>(); 
			cfg.CreateMap<AccountDto, AccountViewModel>();
			cfg.CreateMap<MeterReadingViewModel, MeterReadingDto>(); 
			cfg.CreateMap<MeterReadingDto, MeterReadingViewModel>();
		});
	}
}
