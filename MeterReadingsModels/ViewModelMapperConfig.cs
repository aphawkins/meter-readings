namespace MeterReadingsMvcApp
{
	using AutoMapper;
	using MeterReadingsService.Dto;
	using MeterReadingsModels;

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
