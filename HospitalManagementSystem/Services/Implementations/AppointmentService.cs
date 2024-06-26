﻿using AutoMapper;
using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Repositories.Interfaces;
using HospitalManagementSystem.Services.Interfaces;

namespace HospitalManagementSystem.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task AddAppointmentAsync(CreateAppointmentDTO createAppointmentDto)
        {
            var appointment = _mapper.Map<Appointment>(createAppointmentDto);
            appointment.IsCancelled = false;
            await _appointmentRepository.AddAppointmentAsync(appointment);
        }

        public async Task CancelAppointmentAsync(int id)
        {
            await _appointmentRepository.CancelAppointmentAsync(id);    
        }

        public async Task<IEnumerable<AppointmentDTO>> GetAppointmentsAsync()
        {
            var appointments = await _appointmentRepository.GetAppointmentsAsync();

            var response = appointments?.Select(element =>
            {
                var appointmentDto = new AppointmentDTO
                {
                    Id = element.Id,
                    PatientEmail = element.Patient.Email,
                    DoctorEmail = element.Doctor.Email,
                    AppointmentDate = element.AppointmentDate,
                    IsCancelled = element.IsCancelled
                };
                return appointmentDto;
            });

            return response;
        }


        public async Task<IEnumerable<AppointmentDTO>> GetAppointmentByPatientEmailAsync(string patientEmail)
        {
            var appointments = await _appointmentRepository.GetAppointmentsByPatientEmailAsync(patientEmail);
            return _mapper.Map<IEnumerable<AppointmentDTO>>(appointments);
        }


        public async Task<AppointmentDTO> GetAppointmentByIdAsync(int id)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(id);
            return _mapper.Map<AppointmentDTO>(appointment);
        }

        public async Task UpdateAppointmentAsync(Appointment appointment, int id)
        {
            await _appointmentRepository.UpdateAppointmentAsync(appointment, id);
        }
    }
}
