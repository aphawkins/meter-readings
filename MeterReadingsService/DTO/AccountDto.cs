using System.ComponentModel.DataAnnotations;

namespace MeterReadingsService.Dto
{
	public class AccountDto
	{
		[Required]
		[Range(0, int.MaxValue)]
		public int Id { get; set; }

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }
	}
}