using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services.Implementations;
using HospitalManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HospitalManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {


        private readonly IPrescriptionService _prescriptionservice;
        private readonly UserManager<IdentityUser> _userManager;


        public PrescriptionController(IPrescriptionService prescriptionservice, UserManager<IdentityUser> userManager)
        {
            _prescriptionservice = prescriptionservice;
            _userManager = userManager;
        }


        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult> AddPrescriptionAsync(CreatePrescriptionDTO createprescriptionDto)
        {
            // Validate DoctorId and PatientId
            var doctor = await _userManager.FindByIdAsync(createprescriptionDto.DoctorId);
            var patient = await _userManager.FindByIdAsync(createprescriptionDto.PatientId);

            if (doctor == null)
            {
                return BadRequest("Invalid Doctor Email.");
            }

            if (patient == null)
            {
                return BadRequest("Invalid Patient Email.");
            }

            createprescriptionDto.PatientId = patient.Id;
            createprescriptionDto.DoctorId = doctor.Id;

            await _prescriptionservice.AddPrescriptionAsync(createprescriptionDto);
            return Ok();
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<CreatePrescriptionDTO>>> GetAllPrescriptionsAsync()
        {

            var response = await _prescriptionservice.GetAllPrescriptionsAsync();

            return Ok(response);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePrescription(int id)
        {
            await _prescriptionservice.DeletePrescription(id);
            return Ok();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CreatePrescriptionDTO>> GetPrescriptionByIdAsync(int id)
        {
            var response = await _prescriptionservice.GetPrescriptionByIdAsync(id);
            return Ok(response);
        }

        [HttpGet("search/byPatient/{patientId}")]
        public async Task<ActionResult<IEnumerable<CreatePrescriptionDTO>>> GetPrescriptionsByPatientIdAsync(string patientId)
        {
            var response = await _prescriptionservice.GetPrescriptionsByPatientIdAsync(patientId);

            if (response == null || !response.Any())
            {
                return NotFound("No prescriptions found for the given patient ID");
            }

            return Ok(response);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrescriptionAsync(int id, CreatePrescriptionDTO createPrescriptionDto)
        {
            // Validate DoctorId and PatientId
            var doctor = await _userManager.FindByIdAsync(createPrescriptionDto.DoctorId);
            var patient = await _userManager.FindByIdAsync(createPrescriptionDto.PatientId);

            if (doctor == null)
            {
                return BadRequest("Invalid Doctor ID.");
            }

            if (patient == null)
            {
                return BadRequest("Invalid Patient ID.");
            }

            createPrescriptionDto.PatientId = patient.Id;
            createPrescriptionDto.DoctorId = doctor.Id;

            var prescription = new Prescription
            {
                Id = id,
                PatientId = createPrescriptionDto.PatientId,
                DoctorId = createPrescriptionDto.DoctorId,
                Details = createPrescriptionDto.Details,
                Date = createPrescriptionDto.Date
            };

            await _prescriptionservice.UpdatePrescriptionAsync(prescription, id);
            return Ok();
        }


    }
}
