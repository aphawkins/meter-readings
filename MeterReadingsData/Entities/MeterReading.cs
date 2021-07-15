namespace MeterReadingsData.Entities
{
	using System;

	public class MeterReading
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public Account MyAccount { get; set; }

        public DateTime MeterReadingDateTime { get; set; }

        public int MeterReadingValue { get; set; }
    }
}