﻿using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDTO>> GetAppointmentsAsync();
        Task<AppointmentDTO> GetAppointmentByIdAsync(int id);
        Task AddAppointmentAsync(CreateAppointmentDTO createAppointmentDto);
        Task CancelAppointmentAsync(int id);

        Task<IEnumerable<AppointmentDTO>> GetAppointmentByPatientEmailAsync(string patientEmail);
        Task UpdateAppointmentAsync(Appointment appointment, int id);
    }
}
