using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Repositories.Implementations
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly AppDbContext _context;

        public PrescriptionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddPrescriptionAsync(Prescription prescription)
        {
            try
            {
                Prescription requestBody = new Prescription
                {
                    PatientId = prescription.PatientId,
                    DoctorId = prescription.DoctorId,
                    Details = prescription.Details,
                    Date = prescription.Date
                };

                _context.Prescriptions.Add(requestBody);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding prescription: {ex.Message}");
                throw;
            }
        }

        public async Task<Prescription> GetPrescriptionByIdAsync(int id)
        {
            return await _context.Prescriptions.FindAsync(id);
        }

        public async Task<IEnumerable<Prescription>> GetPrescriptionsByDoctorIdAsync(string doctorId)
        {
            return await _context.Prescriptions
                            .Where(p => p.DoctorId == doctorId)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Prescription>> GetPrescriptionsByPatientIdAsync(string patientId)
        {
            return await _context.Prescriptions
                            .Where(p => p.PatientId == patientId)
                            .ToListAsync();
        }

        public async Task UpdatePrescriptionAsync(Prescription prescription, int id)
        {
            var existingPrescription = await _context.Prescriptions.FindAsync(id);

            if (existingPrescription != null)
            {
                existingPrescription.PatientId = prescription.PatientId;
                existingPrescription.DoctorId = prescription.DoctorId;
                existingPrescription.Details = prescription.Details;
                existingPrescription.Date = prescription.Date;

                _context.Entry(existingPrescription).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePrescription(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);

            if (prescription != null)
            {
                _context.Prescriptions.Remove(prescription);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<Prescription>> GetAllPrescriptionsAsync()
        {
            return await _context.Prescriptions
                .Include(p => p.Patient)
                .Include(p => p.Doctor)
                .ToListAsync();
        }

    }
}
