namespace MeterReadingsData.Models
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations.Schema;

	public class Account
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<MeterReading> MeterReadings { get; set; }
    }
}