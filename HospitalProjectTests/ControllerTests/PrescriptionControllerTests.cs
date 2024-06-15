using HospitalManagementSystem.Controllers;
using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HospitalManagementSystem.Tests.Controllers
{
    public class PrescriptionControllerTests
    {
        private readonly Mock<IPrescriptionService> _mockService;
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly Mock<IMemoryCache> _mockMemoryCache;
        private readonly PrescriptionController _controller;

        public PrescriptionControllerTests()
        {
            _mockService = new Mock<IPrescriptionService>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                null, null, null, null, null, null, null, null);
            _mockMemoryCache = new Mock<IMemoryCache>();
            _controller = new PrescriptionController(_mockService.Object, _mockUserManager.Object, _mockMemoryCache.Object);
        }

        [Fact]
        public async Task AddPrescriptionAsync_ShouldReturnOk()
        {
            // Arrange
            var createPrescriptionDto = new CreatePrescriptionDTO { DoctorId = "D1", PatientId = "P1" };
            var doctor = new IdentityUser { Id = "D1", Email = "doctor@example.com" };
            var patient = new IdentityUser { Id = "P1", Email = "patient@example.com" };

            _mockUserManager.Setup(um => um.FindByIdAsync("D1")).ReturnsAsync(doctor);
            _mockUserManager.Setup(um => um.FindByIdAsync("P1")).ReturnsAsync(patient);

            // Act
            var result = await _controller.AddPrescriptionAsync(createPrescriptionDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockService.Verify(s => s.AddPrescriptionAsync(createPrescriptionDto), Times.Once);
            _mockMemoryCache.Verify(mc => mc.Remove(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetAllPrescriptionsAsync_ShouldReturnCachedPrescriptions()
        {
            // Arrange
            var cachedPrescriptions = new List<PrescriptionDTO> { new PrescriptionDTO() };
            object cacheValue = cachedPrescriptions;

            _mockMemoryCache
                .Setup(m => m.TryGetValue("allPrescriptions", out cacheValue))
                .Returns(true);

            // Act
            var result = await _controller.GetAllPrescriptionsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<PrescriptionDTO>>(okResult.Value);
            Assert.Equal(cachedPrescriptions, returnValue);
        }

        [Fact]
        public async Task GetPrescriptionByIdAsync_ShouldReturnCachedPrescription()
        {
            // Arrange
            var prescriptionId = 1;
            var cachedPrescription = new PrescriptionDTO { Id = prescriptionId };
            object cacheValue = cachedPrescription;

            _mockMemoryCache
                .Setup(m => m.TryGetValue($"prescription_{prescriptionId}", out cacheValue))
                .Returns(true);

            // Act
            var result = await _controller.GetPrescriptionByIdAsync(prescriptionId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<PrescriptionDTO>(okResult.Value);
            Assert.Equal(cachedPrescription, returnValue);
        }

        [Fact]
        public async Task GetPrescriptionsByPatientIdAsync_ShouldReturnCachedPrescriptions()
        {
            // Arrange
            var patientId = "P1";
            var cachedPrescriptions = new List<PrescriptionDTO> { new PrescriptionDTO() };
            object cacheValue = cachedPrescriptions;

            _mockMemoryCache
                .Setup(m => m.TryGetValue($"prescriptions_patient_{patientId}", out cacheValue))
                .Returns(true);

            // Act
            var result = await _controller.GetPrescriptionsByPatientIdAsync(patientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<PrescriptionDTO>>(okResult.Value);
            Assert.Equal(cachedPrescriptions, returnValue);
        }

        [Fact]
        public async Task DeletePrescription_ShouldReturnOk()
        {
            // Arrange
            var prescriptionId = 1;

            // Act
            var result = await _controller.DeletePrescription(prescriptionId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockService.Verify(s => s.DeletePrescription(prescriptionId), Times.Once);
            _mockMemoryCache.Verify(mc => mc.Remove(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task UpdatePrescriptionAsync_ShouldReturnOk()
        {
            // Arrange
            var prescriptionId = 1;
            var createPrescriptionDto = new CreatePrescriptionDTO { DoctorId = "D1", PatientId = "P1", Details = "Details", Date = DateTime.Now };
            var doctor = new IdentityUser { Id = "D1", Email = "doctor@example.com" };
            var patient = new IdentityUser { Id = "P1", Email = "patient@example.com" };

            _mockUserManager.Setup(um => um.FindByIdAsync("D1")).ReturnsAsync(doctor);
            _mockUserManager.Setup(um => um.FindByIdAsync("P1")).ReturnsAsync(patient);

            // Act
            var result = await _controller.UpdatePrescriptionAsync(prescriptionId, createPrescriptionDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockService.Verify(s => s.UpdatePrescriptionAsync(It.IsAny<Prescription>(), prescriptionId), Times.Once);
            _mockMemoryCache.Verify(mc => mc.Remove(It.IsAny<string>()), Times.Exactly(2));
        }
    }
}
