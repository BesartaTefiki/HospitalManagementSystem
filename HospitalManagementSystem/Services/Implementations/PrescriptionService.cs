using AutoMapper;
using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Repositories.Implementations;
using HospitalManagementSystem.Repositories.Interfaces;
using HospitalManagementSystem.Services.Interfaces;

namespace HospitalManagementSystem.Services.Implementations
{
    public class PrescriptionService : IPrescriptionService
    {

        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IMapper _mapper;

        public PrescriptionService(IPrescriptionRepository prescriptionRepository, IMapper mapper)
        {
            _prescriptionRepository = prescriptionRepository;
            _mapper = mapper;
        }

        public async Task AddPrescriptionAsync(CreatePrescriptionDTO createPrescriptionDto)
        {
            var prescription = _mapper.Map<Prescription>(createPrescriptionDto);
            await _prescriptionRepository.AddPrescriptionAsync(prescription);
        }


        public async Task DeletePrescription(int id)
        {
            await _prescriptionRepository.DeletePrescription(id);
        }

        public async Task<IEnumerable<PrescriptionDTO>> GetAllPrescriptionsAsync()
        {
            var prescriptions = await _prescriptionRepository.GetAllPrescriptionsAsync();

            var response = prescriptions?.Select(element => new PrescriptionDTO
            {
                Id = element.Id,
                PatientEmail = element.Patient.Email,
                DoctorEmail = element.Doctor.Email,
                Details = element.Details,
                Date = element.Date
            });

            return response;
        }

        public async Task<PrescriptionDTO> GetPrescriptionByIdAsync(int id)
        {
            var prescriptions = await _prescriptionRepository.GetPrescriptionByIdAsync(id);
            return _mapper.Map<PrescriptionDTO>(prescriptions);
        }

      
        public async Task<IEnumerable<PrescriptionDTO>> GetPrescriptionsByPatientIdAsync(string patientId)
        {
            var prescriptions = await _prescriptionRepository.GetPrescriptionsByPatientIdAsync(patientId);
            return _mapper.Map<IEnumerable<PrescriptionDTO>>(prescriptions);
        }

        public async Task UpdatePrescriptionAsync(Prescription prescription, int id)
        {
            await _prescriptionRepository.UpdatePrescriptionAsync(prescription, id);
        }
    }
}
