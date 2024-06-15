using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HospitalManagementSystem.Tests.Repositories
{
    public class PrescriptionRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly AppDbContext _context;
        private readonly PrescriptionRepository _repository;

        public PrescriptionRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(_options);
            _repository = new PrescriptionRepository(_context);
        }

        [Fact]
        public async Task AddPrescriptionAsync_ShouldAddPrescription()
        {
            // Arrange
            var prescription = new Prescription
            {
                PatientId = "P1",
                DoctorId = "D1",
                Details = "Take two tablets daily",
                Date = DateTime.Now
            };

            // Act
            await _repository.AddPrescriptionAsync(prescription);
            var addedPrescription = await _context.Prescriptions.FirstOrDefaultAsync(p => p.PatientId == "P1");

            // Assert
            Assert.NotNull(addedPrescription);
            Assert.Equal(prescription.PatientId, addedPrescription.PatientId);
        }

        [Fact]
        public async Task GetPrescriptionByIdAsync_ShouldReturnPrescription()
        {
            // Arrange
            var prescription = new Prescription
            {
                PatientId = "P1",
                DoctorId = "D1",
                Details = "Take two tablets daily",
                Date = DateTime.Now
            };
            await _context.Prescriptions.AddAsync(prescription);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetPrescriptionByIdAsync(prescription.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(prescription.Id, result.Id);
        }

        [Fact]
        public async Task GetPrescriptionsByDoctorIdAsync_ShouldReturnPrescriptions()
        {
            // Arrange
            var prescriptions = new List<Prescription>
            {
                new Prescription { PatientId = "P1", DoctorId = "D1", Details = "Details1", Date = DateTime.Now },
                new Prescription { PatientId = "P2", DoctorId = "D1", Details = "Details2", Date = DateTime.Now }
            };
            await _context.Prescriptions.AddRangeAsync(prescriptions);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetPrescriptionsByDoctorIdAsync("D1");

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, p => Assert.Equal("D1", p.DoctorId));
        }

        [Fact]
        public async Task GetPrescriptionsByPatientIdAsync_ShouldReturnPrescriptions()
        {
            // Arrange
            var prescriptions = new List<Prescription>
            {
                new Prescription { PatientId = "P1", DoctorId = "D1", Details = "Details1", Date = DateTime.Now },
                new Prescription { PatientId = "P1", DoctorId = "D2", Details = "Details2", Date = DateTime.Now }
            };
            await _context.Prescriptions.AddRangeAsync(prescriptions);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetPrescriptionsByPatientIdAsync("P1");

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, p => Assert.Equal("P1", p.PatientId));
        }

        [Fact]
        public async Task UpdatePrescriptionAsync_ShouldUpdatePrescription()
        {
            // Arrange
            var prescription = new Prescription
            {
                PatientId = "P1",
                DoctorId = "D1",
                Details = "Take two tablets daily",
                Date = DateTime.Now
            };
            await _context.Prescriptions.AddAsync(prescription);
            await _context.SaveChangesAsync();

            var updatedPrescription = new Prescription
            {
                PatientId = "P1",
                DoctorId = "D1",
                Details = "Take one tablet daily",
                Date = DateTime.Now
            };

            // Act
            await _repository.UpdatePrescriptionAsync(updatedPrescription, prescription.Id);
            var result = await _context.Prescriptions.FindAsync(prescription.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Take one tablet daily", result.Details);
        }

        [Fact]
        public async Task DeletePrescription_ShouldRemovePrescription()
        {
            // Arrange
            var prescription = new Prescription
            {
                PatientId = "P1",
                DoctorId = "D1",
                Details = "Take two tablets daily",
                Date = DateTime.Now
            };
            await _context.Prescriptions.AddAsync(prescription);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeletePrescription(prescription.Id);
            var result = await _context.Prescriptions.FindAsync(prescription.Id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllPrescriptionsAsync_ShouldReturnAllPrescriptions()
        {
            // Arrange
            var prescriptions = new List<Prescription>
            {
                new Prescription { PatientId = "P1", DoctorId = "D1", Details = "Details1", Date = DateTime.Now },
                new Prescription { PatientId = "P2", DoctorId = "D2", Details = "Details2", Date = DateTime.Now }
            };
            await _context.Prescriptions.AddRangeAsync(prescriptions);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllPrescriptionsAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }
    }
}
