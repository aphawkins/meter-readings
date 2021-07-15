namespace MeterReadingsMvcApp.Models
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class AccountViewModel
    {
		[Display(Name = "Account Id")]
		public int Id { get; set; }

		[Display(Name = "First Name")]
        public string FirstName { get; set; }

		[Display(Name = "Last Name")]
		public string LastName { get; set; }
    }
}