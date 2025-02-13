using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PatientAPI.Model;
using PatientAPI.Services;

namespace PatientRepository.Controllers
{
	[ApiController]	
	[Route("api/PANDA/[controller]")]
	public class PatientController : ControllerBase
	{		
		private readonly ILogger<PatientController> _logger;
		private readonly IPatientService _patientService;
		private List<Patient> _patients; 

		public PatientController(ILogger<PatientController> logger , IPatientService patientService)
		{
			_logger = logger;
			_patientService = patientService;
			
		}


		[HttpGet("LoadPatients")]
		public async Task<IActionResult> Get()
		{
			try
			{
				 _patients = await _patientService.LoadPatients();

				if (_patients == null) 
				{
					return BadRequest("Invalid JSON data. Cannot convert to Patient List");
				}
				return Ok(_patients);
				
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

		[HttpGet("GetAllPatients")]
		public async Task<ActionResult<List<Patient>>> GetAllPatients()
		{
			try
			{
				var patients = await _patientService.GetPatients();

				if (patients == null)
				{
					return NotFound();
				}

				return Ok(patients);

			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error");
			}

		}


		[HttpGet("GetPatient/{nhsid}")]	
		public async Task<IActionResult> GetPatient(string nhsid)
		{
			
			var patient = await  _patientService.GetPatientByNHSIDAsync(nhsid);

			if (patient == null)
			{
				return NotFound();
			}

			return Ok(patient);
		}


		[HttpPost("CreatePatient")]
		public async Task<IActionResult> CreatePatient(Patient patientObject)
		{

			if (!ModelState.IsValid)
				return BadRequest(ModelState);


			var patient = await _patientService.AddPatientAsync(patientObject);

			if (patient == null)
			{
				return BadRequest("Patient creation failed!!!");
			}

			return Ok(new
			{
				message = "Patient created successfully!!!",
				id = patient!.nhs_number
			});
		}


		[HttpPut("UpdatePatient/{nhsid}")]		
		public async Task<IActionResult> UpdatePatient([FromRoute] string nhsId, [FromBody] Patient patientObject)
		{

			var patient = await _patientService.UpdatePatientAsync(nhsId, patientObject);

			if (patient == null)
			{
				return BadRequest();
			}

			return Ok(new
			{
				message = "Patient updated  successfully!!!",
				id = patient!.nhs_number
			});
		}



		[HttpDelete("DeletePatient/{nhsid}")]		
		public async Task<IActionResult> DeletePatient([FromRoute] string nhsid)
		{
			var IsDeleted = await _patientService.DeletePatientByNHSIDAsync(nhsid);

			if (!IsDeleted)
			{
				return NotFound();
			}

			return Ok(new
			{
				message = "Patient deleted successfully!!!",
				id = nhsid
			});

		}
	}
}
