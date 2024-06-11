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
            Prescription requestBody = new Prescription();

            requestBody.PatientId = prescription.PatientId;
            requestBody.DoctorId = prescription.DoctorId;
            requestBody.Details = prescription.Details;
            requestBody.Date = prescription.Date;

            _context.Prescriptions.Add(requestBody);
            await _context.SaveChangesAsync();
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
            return await _context.Prescriptions.ToListAsync();
        }
    }
}
