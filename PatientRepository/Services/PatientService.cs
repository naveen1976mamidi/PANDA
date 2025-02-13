using PatientAPI.Filehandlers;
using PatientAPI.Model;
using PatientAPI.NHSNumber;
using PatientAPI.Validations;
using PatientRepository;

namespace PatientAPI.Services
{
	public class PatientService : IPatientService
	{
		public List<Patient> _patientList;

		/// <summary>
		/// /Load all patients from JSON file 
		/// </summary>
		/// <returns></returns>
		public async Task<List<Patient>> LoadPatients()
		{
			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PatientDataFiles", "patients.JSON");

			JsonFileHandler handler = new JsonFileHandler(filePath);
			_patientList = await handler.ReadAsync<List<Patient>>();

			return _patientList;

		}

		/// <summary>
		/// Get all patients 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public async Task<List<Patient>> GetPatients()
		{
			return _patientList;
		}


		/// <summary>
		/// Retrive patient by NHSId
		/// </summary>
		/// <param name="nhsid"></param>
		/// <returns></returns>
		public async Task<Patient> GetPatientByNHSIDAsync(string nhsid)
		{
			await Task.Delay(1);

			return _patientList.FirstOrDefault(p => p.nhs_number == nhsid);
		}

		/// <summary>
		/// Add patient 
		/// </summary>
		/// <param name="patientObj"></param>
		/// <returns></returns>
		public async Task<Patient> AddPatientAsync(Patient patientObj)
		{
			await Task.Delay(1);

			if (Workspace.ValidPostcode(patientObj.postcode))
			{

				var newPatient = new Patient()
				{
					nhs_number = GenerateNewNHSID(),
					name = patientObj.name,
					date_of_birth = patientObj.date_of_birth,
					postcode = patientObj.postcode
				};

				_patientList.Add(newPatient);

				return newPatient;
			}
			else
			{
				return null;

			}
		}



		/// <summary>
		/// Update patient 
		/// </summary>
		/// <param name="nhsid"></param>
		/// <param name="patientObj"></param>
		/// <returns></returns>
		public async Task<Patient?> UpdatePatientAsync(string nhsid, Patient patientObj)
		{
			await Task.Delay(1);

			var result = Workspace.ValidatePatient(patientObj).Trim();

			if (result != null && result.Trim().Length >= 0)
			{
				return null;
			}

			var index = _patientList.FindIndex(i => i.nhs_number == nhsid);

			if (index != -1)
			{
				_patientList[index].name = patientObj.name;
				_patientList[index].date_of_birth = patientObj.date_of_birth;
				_patientList[index].postcode = patientObj.postcode;

				return await GetPatientByNHSIDAsync(_patientList[index].nhs_number);
			}
			else
			{
				return null;
			}
		}



		/// <summary>
		///  Delete patient 
		/// </summary>
		/// <param name="nhsid"></param>
		/// <returns></returns>
		public async Task<bool> DeletePatientByNHSIDAsync(string nhsid)
		{
			await Task.Delay(1);

			var PatientIndex = _patientList.FindIndex(index => index.nhs_number == nhsid);
			if (PatientIndex >= 0)
			{
				_patientList.RemoveAt(PatientIndex);
			}
			return PatientIndex >= 0;
		}


		/// <summary>
		/// Generate new NHS ID 
		/// </summary>
		/// <returns></returns>
		private string GenerateNewNHSID()
		{
			string tempID = string.Empty;

			bool valid = false;

			while (!valid)
			{
				tempID = NHSNumberGenerator.GenerateNHSNumber();

				var PatientIndex = _patientList.FindIndex(index => index.nhs_number == tempID);

				if (PatientIndex <= 0)
				{
					valid = true;
				}
			}

			return tempID;
		}

		/// <summary>
		/// write to file before shutdown
		/// </summary>
		/// <returns></returns>
		public async Task HandleBeforeShutdown()
		{
			if (_patientList.Count > 0)
			{
				string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PatientDataFiles", "patients.JSON");

				if (File.Exists(filePath))
				{
					JsonFileHandler handler = new JsonFileHandler(filePath);
					await handler.WriteAsync(_patientList);

				}
			}

		}



	}
}
