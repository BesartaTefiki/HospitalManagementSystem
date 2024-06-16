using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMemoryCache _memoryCache;
        private readonly string allPrescriptionsCacheKey = "allPrescriptions";

        public PrescriptionController(IPrescriptionService prescriptionService, UserManager<IdentityUser> userManager, IMemoryCache memoryCache)
        {
            _prescriptionService = prescriptionService;
            _userManager = userManager;
            _memoryCache = memoryCache;
        }
        [HttpPost]
        // [Authorize(Roles = "Doctor")]
        public async Task<ActionResult> AddPrescriptionAsync([FromBody] CreatePrescriptionDTO createPrescriptionDto)
        {
            try
            {
                var doctor = await _userManager.FindByEmailAsync(createPrescriptionDto.DoctorEmail);
                var patient = await _userManager.FindByEmailAsync(createPrescriptionDto.PatientEmail);

                if (doctor == null)
                {
                    return BadRequest("Invalid Doctor Email.");
                }

                if (patient == null)
                {
                    return BadRequest("Invalid Patient Email.");
                }

                createPrescriptionDto.PatientId = patient.Id;
                createPrescriptionDto.DoctorId = doctor.Id;

                await _prescriptionService.AddPrescriptionAsync(createPrescriptionDto);
                _memoryCache.Remove("allPrescriptions"); // Invalidate the cache
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here for further analysis
                Console.WriteLine($"Internal server error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetAllPrescriptionsAsync()
        {
            if (!_memoryCache.TryGetValue(allPrescriptionsCacheKey, out IEnumerable<PrescriptionDTO> prescriptions))
            {
                prescriptions = await _prescriptionService.GetAllPrescriptionsAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                _memoryCache.Set(allPrescriptionsCacheKey, prescriptions, cacheEntryOptions);
            }

            return Ok(prescriptions);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePrescription(int id)
        {
            await _prescriptionService.DeletePrescription(id);
            _memoryCache.Remove(allPrescriptionsCacheKey); // Invalidate the cache
            _memoryCache.Remove($"prescription_{id}"); // Invalidate the specific cache
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PrescriptionDTO>> GetPrescriptionByIdAsync(int id)
        {
            var cacheKey = $"prescription_{id}";
            if (!_memoryCache.TryGetValue(cacheKey, out PrescriptionDTO prescription))
            {
                prescription = await _prescriptionService.GetPrescriptionByIdAsync(id);
                if (prescription == null)
                {
                    return NotFound();
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                _memoryCache.Set(cacheKey, prescription, cacheEntryOptions);
            }

            return Ok(prescription);
        }

        [HttpGet("search/byPatient/{patientId}")]
        public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetPrescriptionsByPatientIdAsync(string patientId)
        {
            var cacheKey = $"prescriptions_patient_{patientId}";
            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<PrescriptionDTO> prescriptions))
            {
                prescriptions = await _prescriptionService.GetPrescriptionsByPatientIdAsync(patientId);
                if (prescriptions == null || !prescriptions.Any())
                {
                    return NotFound("No prescriptions found for the given patient ID");
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                _memoryCache.Set(cacheKey, prescriptions, cacheEntryOptions);
            }

            return Ok(prescriptions);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrescriptionAsync(int id, [FromBody] CreatePrescriptionDTO createPrescriptionDto)
        {
            // Validate DoctorEmail and PatientEmail
            var doctor = await _userManager.FindByEmailAsync(createPrescriptionDto.DoctorEmail);
            var patient = await _userManager.FindByEmailAsync(createPrescriptionDto.PatientEmail);

            if (doctor == null)
            {
                return BadRequest("Invalid Doctor Email.");
            }

            if (patient == null)
            {
                return BadRequest("Invalid Patient Email.");
            }

            var prescription = new Prescription
            {
                Id = id,
                PatientId = patient.Id,
                DoctorId = doctor.Id,
                Details = createPrescriptionDto.Details,
                Date = createPrescriptionDto.Date
            };

            await _prescriptionService.UpdatePrescriptionAsync(prescription, id);
            _memoryCache.Remove("allPrescriptions"); // Invalidate the cache
            _memoryCache.Remove($"prescription_{id}"); // Invalidate the specific cache
            return Ok();
        }

    }
}
