using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Repositories.Interfaces
{
    public interface IPrescriptionRepository
    {
        Task AddPrescriptionAsync(Prescription prescription);
        Task<Prescription> GetPrescriptionByIdAsync(int id);
        Task<IEnumerable<Prescription>> GetPrescriptionsByPatientIdAsync(string patientId);
        Task<IEnumerable<Prescription>> GetAllPrescriptionsAsync();
        Task UpdatePrescriptionAsync(Prescription prescription, int id);
        Task DeletePrescription(int id);
    }
}
