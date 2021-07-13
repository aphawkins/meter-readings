namespace MeterReadingsMvcApp.ViewModels
{
	using System;

	public class MeterReadingViewModel
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public AccountViewModel MyAccount { get; set; }

        public DateTime MeterReadingDateTime { get; set; }

        public int MeterReadingValue { get; set; }
    }
}