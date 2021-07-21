namespace MeterReadings.Dto
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class MeterReadingDto
	{
		[Range(0, int.MaxValue)]
		public int? Id { get; set; }

		[Range(0, int.MaxValue)]
		[Required]
		public int AccountId { get; set; }

		[DataType(DataType.DateTime)]
		[Required]
		public DateTime MeterReadingDateTime { get; set; }

		[Range(0, 99999)]
		[Required]
		public int MeterReadingValue { get; set; }
	}
}