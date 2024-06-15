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
    public class AppointmentControllerTests
    {
        private readonly Mock<IAppointmentService> _mockService;
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly Mock<IMemoryCache> _mockMemoryCache;
        private readonly AppointmentController _controller;

        public AppointmentControllerTests()
        {
            _mockService = new Mock<IAppointmentService>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                null, null, null, null, null, null, null, null);
            _mockMemoryCache = new Mock<IMemoryCache>();
            _controller = new AppointmentController(_mockService.Object, _mockUserManager.Object, _mockMemoryCache.Object);
        }

        [Fact]
        public async Task AddAppointmentAsync_ShouldReturnOk()
        {
            // Arrange
            var createAppointmentDto = new CreateAppointmentDTO { DoctorEmail = "doctor@example.com", PatientEmail = "patient@example.com" };
            var doctor = new IdentityUser { Id = "D1", Email = "doctor@example.com" };
            var patient = new IdentityUser { Id = "P1", Email = "patient@example.com" };

            _mockUserManager.Setup(um => um.FindByEmailAsync("doctor@example.com")).ReturnsAsync(doctor);
            _mockUserManager.Setup(um => um.FindByEmailAsync("patient@example.com")).ReturnsAsync(patient);

            // Act
            var result = await _controller.AddAppointmentAsync(createAppointmentDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockService.Verify(s => s.AddAppointmentAsync(createAppointmentDto), Times.Once);
            _mockMemoryCache.Verify(mc => mc.Remove(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetAppointmentsAsync_ShouldReturnCachedAppointments()
        {
            // Arrange
            var cachedAppointments = new List<AppointmentDTO> { new AppointmentDTO() };
            object cacheValue = cachedAppointments;

            _mockMemoryCache
                .Setup(m => m.TryGetValue("allAppointments", out cacheValue))
                .Returns(true);

            // Act
            var result = await _controller.GetAppointmentsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<AppointmentDTO>>(okResult.Value);
            Assert.Equal(cachedAppointments, returnValue);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_ShouldReturnCachedAppointment()
        {
            // Arrange
            var appointmentId = 1;
            var cachedAppointment = new AppointmentDTO { Id = appointmentId };
            object cacheValue = cachedAppointment;

            _mockMemoryCache
                .Setup(m => m.TryGetValue($"appointment_{appointmentId}", out cacheValue))
                .Returns(true);

            // Act
            var result = await _controller.GetAppointmentByIdAsync(appointmentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<AppointmentDTO>(okResult.Value);
            Assert.Equal(cachedAppointment, returnValue);
        }

        [Fact]
        public async Task GetAppointmentByPatientIdAsync_ShouldReturnCachedAppointments()
        {
            // Arrange
            var patientId = "P1";
            var cachedAppointments = new List<AppointmentDTO> { new AppointmentDTO() };
            object cacheValue = cachedAppointments;

            _mockMemoryCache
                .Setup(m => m.TryGetValue($"appointments_patient_{patientId}", out cacheValue))
                .Returns(true);

            // Act
            var result = await _controller.GetAppointmentByPatientIdAsync(patientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<AppointmentDTO>>(okResult.Value);
            Assert.Equal(cachedAppointments, returnValue);
        }

        [Fact]
        public async Task CancelAppointment_ShouldReturnOk()
        {
            // Arrange
            var appointmentId = 1;

            // Act
            var result = await _controller.CancelAppointmentAsync(appointmentId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockService.Verify(s => s.CancelAppointmentAsync(appointmentId), Times.Once);
            _mockMemoryCache.Verify(mc => mc.Remove(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task UpdateAppointmentAsync_ShouldReturnNoContent()
        {
            // Arrange
            var appointmentId = 1;
            var createAppointmentDto = new CreateAppointmentDTO { DoctorEmail = "doctor@example.com", PatientEmail = "patient@example.com", AppointmentDate = DateTime.Now };
            var doctor = new IdentityUser { Id = "D1", Email = "doctor@example.com" };
            var patient = new IdentityUser { Id = "P1", Email = "patient@example.com" };

            _mockUserManager.Setup(um => um.FindByEmailAsync("doctor@example.com")).ReturnsAsync(doctor);
            _mockUserManager.Setup(um => um.FindByEmailAsync("patient@example.com")).ReturnsAsync(patient);

            // Act
            var result = await _controller.UpdateAppointmentAsync(appointmentId, createAppointmentDto);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            _mockService.Verify(s => s.UpdateAppointmentAsync(It.IsAny<Appointment>(), appointmentId), Times.Once);
            _mockMemoryCache.Verify(mc => mc.Remove(It.IsAny<string>()), Times.Exactly(2));
        }
    }
}
