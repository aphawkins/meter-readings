namespace MeterReadings.DB.Entities
{
	using System;
	using System.ComponentModel.DataAnnotations.Schema;

	public class MeterReading
    {
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

        public int AccountId { get; set; }

        public Account MyAccount { get; set; }

        public DateTime MeterReadingDateTime { get; set; }

        public int MeterReadingValue { get; set; }
    }
}