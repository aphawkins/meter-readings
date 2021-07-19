namespace MeterReadingsModels
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class MeterReadingViewModel
    {
		[Display(Name = "Meter Reading Id")]
		[Range(0, int.MaxValue)]
		public int Id { get; set; }

		[Display(Name = "Account Id")]
		[Required]
		[Range(0, int.MaxValue)]
		public int AccountId { get; set; }

		[Display(Name = "Reading Time")]
		[DataType(DataType.DateTime)]
		[Required]
		public DateTime MeterReadingDateTime { get; set; }

		[Display(Name = "Reading Value")]
		[Range(0, 99999)]
		[Required]
		public int MeterReadingValue { get; set; }
    }
}