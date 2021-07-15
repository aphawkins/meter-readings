namespace MeterReadingsMvcApp.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class MeterReadingViewModel
    {
		[Display(Name = "Meter Reading Id")]
		public int Id { get; set; }

		[Display(Name = "Account Id")]
		public int AccountId { get; set; }

		[Display(Name = "Reading Time")]
		[DataType(DataType.DateTime)]
		public DateTime MeterReadingDateTime { get; set; }

		[Display(Name = "Reading Value")]
		[Range(0, 99999)]
		public int MeterReadingValue { get; set; }
    }
}