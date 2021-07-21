namespace MeterReadings.Models
{
	using System.ComponentModel.DataAnnotations;

	public class AccountViewModel
    {
		[Display(Name = "Account Id")]
		[Range(0, int.MaxValue)]
		public int Id { get; set; }

		[Display(Name = "First Name")]
		[Required]
        public string FirstName { get; set; }

		[Display(Name = "Last Name")]
		[Required]
		public string LastName { get; set; }
    }
}