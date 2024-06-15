using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services.Implementations;
using HospitalManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMemoryCache _memoryCache;
        private readonly string allAppointmentsCacheKey = "allAppointments";

        public AppointmentController(IAppointmentService appointmentService, UserManager<IdentityUser> userManager, IMemoryCache memoryCache)
        {
            _appointmentService = appointmentService;
            _userManager = userManager;
            _memoryCache = memoryCache;
        }

        [HttpPost]
        //[Authorize(Roles = "Patient")]
        public async Task<ActionResult> AddAppointmentAsync([FromBody] CreateAppointmentDTO createAppointmentDto)
        {
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

            await _appointmentService.AddAppointmentAsync(createAppointmentDto);
            _memoryCache.Remove(allAppointmentsCacheKey); // Invalidate the cache
            return Ok();
        }
        [HttpGet]
        //[Authorize(Roles = "Patient")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAppointmentsAsync()
        {
            if (!_memoryCache.TryGetValue(allAppointmentsCacheKey, out IEnumerable<AppointmentDTO> appointments))
            {
                appointments = await _appointmentService.GetAppointmentsAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                _memoryCache.Set(allAppointmentsCacheKey, appointments, cacheEntryOptions);
            }

            return Ok(appointments);
        }


        [HttpDelete("{id}")]
        //[Authorize(Roles = "Patient")]
        public async Task<ActionResult> CancelAppointmentAsync(int id)
        {
            await _appointmentService.CancelAppointmentAsync(id);
            _memoryCache.Remove(allAppointmentsCacheKey); // Invalidate the cache
            _memoryCache.Remove($"appointment_{id}"); // Invalidate the specific cache
            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<AppointmentDTO>> GetAppointmentByIdAsync(int id)
        {
            var cacheKey = $"appointment_{id}";
            if (!_memoryCache.TryGetValue(cacheKey, out AppointmentDTO appointment))
            {
                appointment = await _appointmentService.GetAppointmentByIdAsync(id);
                if (appointment == null)
                {
                    return NotFound();
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                _memoryCache.Set(cacheKey, appointment, cacheEntryOptions);
            }

            return Ok(appointment);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Patient")]
        public async Task<IActionResult> UpdateAppointmentAsync(int id, CreateAppointmentDTO createAppointmentDto)
        {
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

            await _appointmentService.UpdateAppointmentAsync(appointment, id);
            _memoryCache.Remove(allAppointmentsCacheKey); // Invalidate the cache
            _memoryCache.Remove($"appointment_{id}"); // Invalidate the specific cache
            return NoContent();
        }

        [HttpGet("search/byPatient/{patientId}")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAppointmentByPatientIdAsync(string patientId)
        {
            var cacheKey = $"appointments_patient_{patientId}";
            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<AppointmentDTO> appointments))
            {
                appointments = await _appointmentService.GetAppointmentByPatientIdAsync(patientId);
                if (appointments == null || !appointments.Any())
                {
                    return NotFound("No appointments found for the given patient ID");
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                _memoryCache.Set(cacheKey, appointments, cacheEntryOptions);
            }

            return Ok(appointments);
        }
    }
}
