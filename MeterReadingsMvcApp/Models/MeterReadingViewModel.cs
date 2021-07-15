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

        public AccountViewModel MyAccount { get; set; }

		[Display(Name = "Reading Time")]
		public DateTime MeterReadingDateTime { get; set; }

		[Display(Name = "Reading Value")]
		public int MeterReadingValue { get; set; }
    }
}