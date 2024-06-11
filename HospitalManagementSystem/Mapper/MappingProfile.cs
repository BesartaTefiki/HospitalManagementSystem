using AutoMapper;
using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Appointment, AppointmentDTO>();
            CreateMap<CreateAppointmentDTO, Appointment>();

            CreateMap<Prescription, PrescriptionDTO>();
            CreateMap<CreatePrescriptionDTO, Prescription>();
        }
    }
}
