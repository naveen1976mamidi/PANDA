using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PatientAPI.Model
{
	public enum AppointmentStatus
	{
		Active,		
		Missed,
		Cancelled
	}

	public class Appointment
	{
		
		public string patient { get; set; }

		
		public string status { get; set; }

		
		public DateTime time { get; set; }

		
		public string duration { get; set; } 

		
		public string clinician { get; set; }

		
		public string department { get; set; }

		
		//[RegularExpression(@"^([A-Z]{1,2}[0-9][A-Z0-9]?\s*[0-9][A-Z]{2})$", ErrorMessage = "Invalid postcode")] 
		public string postcode { get; set; }

		
		public string  id { get; set; }	
	}
}
















