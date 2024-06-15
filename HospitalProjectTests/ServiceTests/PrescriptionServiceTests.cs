using AutoMapper;
using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Repositories.Interfaces;
using HospitalManagementSystem.Services.Implementations;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HospitalManagementSystem.Tests.Services
{
    public class PrescriptionServiceTests
    {
        private readonly Mock<IPrescriptionRepository> _prescriptionRepositoryMock;
        private readonly IMapper _mapper;
        private readonly PrescriptionService _prescriptionService;

        public PrescriptionServiceTests()
        {
            _prescriptionRepositoryMock = new Mock<IPrescriptionRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreatePrescriptionDTO, Prescription>();
                cfg.CreateMap<Prescription, PrescriptionDTO>();
            });
            _mapper = config.CreateMapper();

            _prescriptionService = new PrescriptionService(_prescriptionRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task AddPrescriptionAsync_ShouldCallRepository()
        {
            // Arrange
            var createPrescriptionDto = new CreatePrescriptionDTO
            {
                PatientId = "P1",
                DoctorId = "D1",
                Details = "Take two tablets daily",
                Date = DateTime.Now
            };

            // Act
            await _prescriptionService.AddPrescriptionAsync(createPrescriptionDto);

            // Assert
            _prescriptionRepositoryMock.Verify(repo => repo.AddPrescriptionAsync(It.IsAny<Prescription>()), Times.Once);
        }

        [Fact]
        public async Task DeletePrescription_ShouldCallRepository()
        {
            // Arrange
            var prescriptionId = 1;

            // Act
            await _prescriptionService.DeletePrescription(prescriptionId);

            // Assert
            _prescriptionRepositoryMock.Verify(repo => repo.DeletePrescription(prescriptionId), Times.Once);
        }

        [Fact]
        public async Task GetAllPrescriptionsAsync_ShouldReturnPrescriptionDTOs()
        {
            // Arrange
            var prescriptions = new List<Prescription>
            {
                new Prescription { Id = 1, PatientId = "P1", DoctorId = "D1", Details = "Details1", Date = DateTime.Now },
                new Prescription { Id = 2, PatientId = "P2", DoctorId = "D2", Details = "Details2", Date = DateTime.Now }
            };
            _prescriptionRepositoryMock.Setup(repo => repo.GetAllPrescriptionsAsync()).ReturnsAsync(prescriptions);

            // Act
            var result = await _prescriptionService.GetAllPrescriptionsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, item => Assert.IsType<PrescriptionDTO>(item));
        }

        [Fact]
        public async Task GetPrescriptionByIdAsync_ShouldReturnPrescriptionDTO()
        {
            // Arrange
            var prescription = new Prescription
            {
                Id = 1,
                PatientId = "P1",
                DoctorId = "D1",
                Details = "Take two tablets daily",
                Date = DateTime.Now
            };
            _prescriptionRepositoryMock.Setup(repo => repo.GetPrescriptionByIdAsync(prescription.Id)).ReturnsAsync(prescription);

            // Act
            var result = await _prescriptionService.GetPrescriptionByIdAsync(prescription.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PrescriptionDTO>(result);
            Assert.Equal(prescription.Id, result.Id);
        }

        [Fact]
        public async Task GetPrescriptionsByPatientIdAsync_ShouldReturnPrescriptionDTOs()
        {
            // Arrange
            var patientId = "P1";
            var prescriptions = new List<Prescription>
            {
                new Prescription { Id = 1, PatientId = "P1", DoctorId = "D1", Details = "Details1", Date = DateTime.Now },
                new Prescription { Id = 2, PatientId = "P1", DoctorId = "D2", Details = "Details2", Date = DateTime.Now }
            };
            _prescriptionRepositoryMock.Setup(repo => repo.GetPrescriptionsByPatientIdAsync(patientId)).ReturnsAsync(prescriptions);

            // Act
            var result = await _prescriptionService.GetPrescriptionsByPatientIdAsync(patientId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, item => Assert.IsType<PrescriptionDTO>(item));
        }

        [Fact]
        public async Task UpdatePrescriptionAsync_ShouldCallRepository()
        {
            // Arrange
            var prescription = new Prescription
            {
                Id = 1,
                PatientId = "P1",
                DoctorId = "D1",
                Details = "Take two tablets daily",
                Date = DateTime.Now
            };
            var prescriptionId = 1;

            // Act
            await _prescriptionService.UpdatePrescriptionAsync(prescription, prescriptionId);

            // Assert
            _prescriptionRepositoryMock.Verify(repo => repo.UpdatePrescriptionAsync(prescription, prescriptionId), Times.Once);
        }
    }
}
