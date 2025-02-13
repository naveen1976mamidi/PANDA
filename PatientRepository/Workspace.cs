using PatientAPI.Model;
using PatientAPI.NHSNumber;
using PatientAPI.Validations;

namespace PatientRepository
{
	public static class Workspace
	{

		/// <summary>
		/// Validate patient  NHSNumber and Postcode 
		/// </summary>
		/// <param name="patientObj"></param>
		/// <returns></returns>
		public static  string ValidatePatient(Patient patientObj)
		{
			string strError = string.Empty;

			if (!NHSNumberChecker.IsValidNHSNumber(patientObj.nhs_number.ToString()))
			{
				strError = strError + " Invalid NHS Number, ";
			}

			if (!ValidPostcode(patientObj.postcode))
			{
				strError = strError + " Invalid Postcode ";
			}

			return strError;
		}





		/// <summary>
		/// Validate Postcode 
		/// </summary>
		/// <returns></returns>
		public  static bool ValidPostcode(string postcode)
		{
			return PostCodeChecker.IsValidUKPostcode(postcode);
		}


		/// <summary>
		/// Validate appointment PAtient number  and Postcode 
		/// </summary>
		/// <param name="appointmentObjj"></param>
		/// <returns></returns>
		public static string ValidateAppointment(Appointment appointmentObjj)
		{
			string strError = string.Empty;

			if (!NHSNumberChecker.IsValidNHSNumber(appointmentObjj.patient.ToString()))
			{
				strError = strError + " Invalid NHS Number, ";
			}

			if (!ValidPostcode(appointmentObjj.postcode))
			{
				strError = strError + " Invalid Postcode ";
			}

			return strError;
		}

	}
}
