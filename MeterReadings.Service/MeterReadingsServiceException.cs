namespace MeterReadings.Service
{
	using System;

	public class MeterReadingsServiceException : Exception
	{
		public MeterReadingsServiceException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
