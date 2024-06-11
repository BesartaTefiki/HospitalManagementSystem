using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Services.Interfaces
{
    public interface IPrescriptionService
    {

        Task AddPrescriptionAsync(CreatePrescriptionDTO createPrescriptionDto);
        Task<PrescriptionDTO> GetPrescriptionByIdAsync(int id);
        Task<IEnumerable<PrescriptionDTO>> GetPrescriptionsByPatientIdAsync(string patientId);
        Task<IEnumerable<PrescriptionDTO>> GetAllPrescriptionsAsync();
        Task UpdatePrescriptionAsync(Prescription prescription, int id);
        Task DeletePrescription(int id);
    }
}
