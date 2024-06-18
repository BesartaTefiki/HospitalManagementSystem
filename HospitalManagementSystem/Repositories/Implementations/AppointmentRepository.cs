using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Repositories.Implementations
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAppointmentAsync(Appointment appointment)
        {
            Appointment requestBody = new Appointment();

            requestBody.PatientId = appointment.PatientId;
            requestBody.DoctorId = appointment.DoctorId;
            requestBody.AppointmentDate = appointment.AppointmentDate;
            requestBody.IsCancelled = false;

            _context.Appointments.Add(requestBody);
            await _context.SaveChangesAsync();
        }

        public async Task CancelAppointmentAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Appointment> GetAppointmentByIdAsync(int id)
        {
            return await _context.Appointments.FindAsync(id);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientEmailAsync(string patientEmail)
        {
            var patient = await _context.Users.FirstOrDefaultAsync(u => u.Email == patientEmail);
            if (patient == null)
            {
                return new List<Appointment>();
            }

            return await _context.Appointments
                                 .Where(a => a.PatientId == patient.Id)
                                 .Include(a => a.Patient)
                                 .Include(a => a.Doctor)
                                 .ToListAsync();
        }


        public async Task<IEnumerable<Appointment>> GetAppointmentsAsync()
        {
            return await _context.Appointments
             .Include(a => a.Patient)
             .Include(a => a.Doctor)
             .ToListAsync();
        }

        public async Task UpdateAppointmentAsync(Appointment appointment, int id)
        {
            var existingAppointment = await _context.Appointments.FindAsync(id);

            if (existingAppointment != null)
            {
                existingAppointment.PatientId = appointment.PatientId;
                existingAppointment.DoctorId = appointment.DoctorId;
                existingAppointment.AppointmentDate = appointment.AppointmentDate;
                existingAppointment.IsCancelled = appointment.IsCancelled;

                _context.Entry(existingAppointment).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}
