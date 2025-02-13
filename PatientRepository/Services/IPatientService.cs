using PatientAPI.Model;

namespace PatientAPI.Services
{
	public interface IPatientService
	{

		Task<Patient> GetPatientByNHSIDAsync(string nhsid);

		Task<Patient> AddPatientAsync(Patient patientObj);

		Task<Patient?> UpdatePatientAsync(string  nhsid, Patient patientObj);

		Task<bool> DeletePatientByNHSIDAsync(string nhsid);

				
		Task<List<Patient>>  GetPatients();

		Task<List<Patient>> LoadPatients();

		Task HandleBeforeShutdown();
	}
}
