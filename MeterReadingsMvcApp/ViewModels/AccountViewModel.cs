namespace MeterReadingsMvcApp.ViewModels
{
	using System.Collections.Generic;

	public class AccountViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<MeterReadingViewModel> MeterReadings { get; set; }
    }
}