using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PatientAPI.Model;
using PatientAPI.Services;
using PatientRepository;

namespace PatientAPI.Controllers
{
	[Route("api/PANDA/[controller]")]
	[ApiController]
	public class AppointmentController : ControllerBase
	{
		private IAppointmentService _appointmentService;
		private readonly ILogger<AppointmentController> _logger;
		private List<Appointment> _appointments;

		public AppointmentController(ILogger<AppointmentController> logger, IAppointmentService patientService)
		{
			_logger = logger;
			_appointmentService = patientService;
		}


		[HttpGet("LoadAppointments")]
		public async Task<IActionResult> LoadAppointments()
		{
			try
			{
				_appointments = await _appointmentService.LoadAppointments();

				if (_appointments == null)
				{
					return BadRequest("Invalid JSON data. Cannot convert to Appointment List");
				}
				return Ok(_appointments);

			}
			catch (JsonReaderException ex)
			{
				return BadRequest($"Invalid JSON format: {ex.Message}");
			}
			catch (Exception ex)
			{
				//other
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpGet("GetAllAppointments")]
		public async Task<ActionResult<List<Appointment>>> GetAllAppointments()
		{
			try
			{
				var appointments = await _appointmentService.GetAppointments();

				if (appointments == null)
				{
					return NotFound();
				}

				return Ok(appointments);

			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error");
			}

		}

		[HttpGet("GetAllActiveAppointments/{patientid}")]
		public async Task<ActionResult<List<Appointment>>> GetAllActiveAppointments(string patientid)
		{
			try
			{
				var appointments = await _appointmentService.GetAllActiveAppointments(patientid);

				if (appointments == null)
				{
					return NotFound();
				}

				return Ok(appointments);

			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error");
			}

		}




		[HttpGet("GetAppointment/{id}")]
		public async Task<IActionResult> GetAppointment(string id)
		{
			var appointment = await _appointmentService.GetAppointmentAsync(id);

			if (appointment == null)
			{
				return NotFound();
			}
			return Ok(appointment);
		}


		[HttpPost("CreateAppointment")]
		public async Task<ActionResult> CreateAppointment(Appointment appointment)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			appointment.id = Guid.NewGuid().ToString();

			var tempappointment = await _appointmentService.AddAppointmentAsync(appointment);


			if (tempappointment == null)
			{
				return BadRequest("Patient creation failed!!!");
			}

			return Ok(new
			{
				message = "Appointment created successfully!!!",
				id = tempappointment!.patient,
			});
		}



		[HttpPut("UpdateAppointment/{id}")]
		public async Task<IActionResult> UpdateAppointment([FromRoute] string id, [FromBody] Appointment appointment)
		{
			if (id != appointment.id)
			{
				return BadRequest("IDs do not match.");
			}

			var resultAppointment = await _appointmentService.UpdateAppointmentAsync(id, appointment);

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (resultAppointment == null)
			{
				return BadRequest();
			}

			return Ok(new
			{
				message = "Appointment  updated  successfully!!!",
				id = resultAppointment!.patient
			});

		}




		[HttpPatch("CancelAppointment/{id}")]

		public async Task<IActionResult> CancelAppointment(string id)
		{
			var appointment = _appointments.Find(a => a.id == id);

			if (appointment == null)
			{
				return NotFound();
			}

			var cancelled = await _appointmentService.CancelAppointmentAsync(id);
			if (!cancelled)
			{
				return BadRequest();
			}

			return Ok(new
			{
				message = "Patient Appointment  successfully Cancelled!!!",
				id
			});

		}


	}
}
