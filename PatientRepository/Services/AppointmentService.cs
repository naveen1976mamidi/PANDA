using PatientAPI.Filehandlers;
using PatientAPI.Model;
using PatientRepository;

namespace PatientAPI.Services
{
	public class AppointmentService : IAppointmentService
	{

		private List<Appointment> _appointments;

	
		/// <summary>
		/// /Load all Appointments from JSON file 
		/// </summary>
		/// <returns></returns>
		public async Task<List<Appointment>> LoadAppointments()
		{
			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PatientDataFiles", "appointments.JSON");

			JsonFileHandler handler = new JsonFileHandler(filePath);
			_appointments = await handler.ReadAsync<List<Appointment>>();

			return _appointments;

		}


		/// <summary>
		/// Get all Appointments 
		/// </summary>
		/// <returns></returns>
		public async Task<List<Appointment>> GetAppointments()
		{
			return _appointments;
		}


		/// <summary>
		/// Get appointment by patient 
		/// </summary>
		/// <param name="patientId"></param>
		/// <returns></returns>
		public async Task<List<Appointment>> GetAppointmentAsync(string patientId)
		{
			await Task.Delay(1);
			return _appointments.FindAll(p => p.patient == patientId);
		}



		/// <summary>
		/// Get appointment by patient 
		/// </summary>
		/// <param name="patientId"></param>
		/// <returns></returns>
		public async Task<List<Appointment>> GetAllActiveAppointments(string patientId)
		{
			await Task.Delay(1);
			return _appointments.FindAll(p => p.patient == patientId && p.status == AppointmentStatus.Active.ToString());
		}


		/// <summary>
		/// Create new  appointment 
		/// </summary>
		/// <param name="appointment"></param>
		/// <returns></returns>
		public async Task<Appointment> AddAppointmentAsync(Appointment appointment)
		{
			await Task.Delay(1);

			var newAppointment = new Appointment()
			{
				id = new Guid().ToString(),
				status = AppointmentStatus.Active.ToString(),
				time = appointment.time,
				duration = appointment.duration,
				clinician = appointment.clinician,
				department = appointment.department,
				postcode = appointment.postcode
			};

			_appointments.Add(newAppointment);

			return newAppointment;

		}

		/// <summary>
		/// Cancel appointment 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<bool> CancelAppointmentAsync(string id)
		{

			bool isCancelled = false;

			await Task.Delay(1);

			var appointment =  _appointments.Find(a => a.id == id);


			if (appointment != null)
			{
				appointment.status = AppointmentStatus.Cancelled.ToString();
				isCancelled = true;
			}

			return isCancelled;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="appointment"></param>
		/// <returns></returns>
		public async Task<Appointment?> UpdateAppointmentAsync( string id, Appointment appointment)
		{
			await Task.Delay(1);


			var error = Workspace.ValidateAppointment(appointment).Trim();

			if (error != null && error.Trim().Length >= 0)
			{
				return null;
			}

			var existingAppointment = _appointments.Find(a => a.id == id);

			if (existingAppointment == null)
			{
				return null;
			}


			var result = _appointments.Find(a => a.id == id);

			if (result != null && result.status == AppointmentStatus.Active.ToString() && result.time > appointment.time)
			{
				result.time = appointment.time;
				result.duration = appointment.duration;
				result.clinician = appointment.clinician;
				result.department = appointment.department;
				result.status = appointment.status;
			}			

			return appointment;
		}

		/// <summary>
		/// write to file before shutdown
		/// </summary>
		/// <returns></returns>
		public async Task HandleBeforeShutdown()
		{
			if (_appointments.Count > 0)
			{
				string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PatientDataFiles", "appointments.JSON");

				if (File.Exists(filePath))
				{
					JsonFileHandler handler = new JsonFileHandler(filePath);
					await handler.WriteAsync(_appointments);

				}
			}

		}


	}


}


