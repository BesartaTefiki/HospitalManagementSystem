using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAppointmentsAsync();
        Task<Appointment> GetAppointmentByIdAsync(int id);
        Task AddAppointmentAsync(Appointment appointment);
        Task UpdateAppointmentAsync(Appointment appointment, int id);
        Task CancelAppointmentAsync(int id);

        Task<IEnumerable<Appointment>> GetAppointmentsByPatientEmailAsync(string patientEmail);
    }
}
