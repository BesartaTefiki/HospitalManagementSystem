using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services.Implementations;
using HospitalManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using System.Data;
using System.Security.Claims;

namespace HospitalManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentservice;
        private readonly UserManager<IdentityUser> _userManager;
        

        public AppointmentController(IAppointmentService appoinmentnservice, UserManager<IdentityUser> userManager)
        {
            _appointmentservice = appoinmentnservice;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult> AddAppointmentAsync(CreateAppointmentDTO createAppointmentDto)
        {
            // Validate DoctorId and PatientId
            var doctor = await _userManager.FindByEmailAsync(createAppointmentDto.DoctorEmail);
            var patient = await _userManager.FindByEmailAsync(createAppointmentDto.PatientEmail);

            if (doctor == null)
            {
                return BadRequest("Invalid Doctor Email.");
            }

            if (patient == null)
            {
                return BadRequest("Invalid Patient Email.");
            }

            createAppointmentDto.PatientId = patient.Id;
            createAppointmentDto.DoctorId = doctor.Id;

            await _appointmentservice.AddAppointmentAsync(createAppointmentDto);
            return Ok();

        }


        [HttpGet]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<IEnumerable<CreateAppointmentDTO>>> GetAppointmentsAsync()
        {
           
            var response = await _appointmentservice.GetAppointmentsAsync();

            return Ok(response);
        }

        [HttpDelete("/{id}")]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult> CancelAppointmentAsync(int id)
        {
            await _appointmentservice.CancelAppointmentAsync(id);
            return Ok();
        }

        [HttpGet("/{id}")]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<CreateAppointmentDTO>> GetAppointmentByIdAsync(int id)
        {
            var response = await _appointmentservice.GetAppointmentByIdAsync(id);
            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> UpdateAppointmentAsync(int id, CreateAppointmentDTO createAppointmentDto)
        {
            // Validate DoctorId and PatientId
            var doctor = await _userManager.FindByEmailAsync(createAppointmentDto.DoctorEmail);
            var patient = await _userManager.FindByEmailAsync(createAppointmentDto.PatientEmail);

            if (doctor == null)
            {
                return BadRequest("Invalid Doctor Email.");
            }

            if (patient == null)
            {
                return BadRequest("Invalid Patient Email.");
            }

            createAppointmentDto.PatientId = patient.Id;
            createAppointmentDto.DoctorId = doctor.Id;

            var appointment = new Appointment
            {
                Id = id,
                PatientId = createAppointmentDto.PatientId,
                DoctorId = createAppointmentDto.DoctorId,
                AppointmentDate = createAppointmentDto.AppointmentDate,
                IsCancelled = false

            };

            await _appointmentservice.UpdateAppointmentAsync(appointment, id);
            return NoContent();
        }
        [HttpGet("search/byPatient/{patientId}")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAppointmentByPatientIdAsync(string patientId)
        {
            var response = await _appointmentservice.GetAppointmentByPatientIdAsync(patientId);

            if (response == null || !response.Any())
            {
                return NotFound("No appointments found for the given patient ID");
            }
            return Ok(response);
        }


    }
}
