using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PatientAPI.Model
{
	public class Patient
	{
		
		public string  nhs_number { get; set; }

			
		public string name { get; set; }

		//[RegularExpression(@"^[0-9]{4}-[0-9]{1,2}-[0-9]{1,2}$")]
		public string date_of_birth { get; set; }

		//[RegularExpression(@"^([A-Z]{1,2}\d[A-Z\d]? \d[A-Z]{2})$")]
		public string postcode { get; set; }

	}

}