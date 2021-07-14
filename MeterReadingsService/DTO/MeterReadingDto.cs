namespace MeterReadingsService.Dto
{
	using System;

	public class MeterReadingDto
	{
		public int Id { get; set; }

		public int AccountId { get; set; }

		public DateTime MeterReadingDateTime { get; set; }

		public int MeterReadingValue { get; set; }
	}
}