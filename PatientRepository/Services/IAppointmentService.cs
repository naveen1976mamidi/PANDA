using PatientAPI.Model;

namespace PatientAPI.Services
{
	public interface IAppointmentService
	{
		Task<List<Appointment>> GetAppointmentAsync(string id);

		Task<List<Appointment>> GetAllActiveAppointments(string patientId);

		Task<Appointment> AddAppointmentAsync(Appointment appointment);

		Task<Appointment?> UpdateAppointmentAsync(string id ,Appointment appointment);

		Task<bool> CancelAppointmentAsync(string id);

		Task<List<Appointment>> LoadAppointments();

		Task<List<Appointment>> GetAppointments();

		Task HandleBeforeShutdown();

	}
}
