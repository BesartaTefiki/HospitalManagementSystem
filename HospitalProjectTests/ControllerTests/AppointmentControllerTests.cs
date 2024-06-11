using HospitalManagementSystem.Controllers;
using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class AppointmentControllerTests
{
    private readonly Mock<IAppointmentService> _appointmentServiceMock;
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly AppointmentController _controller;

    public AppointmentControllerTests()
    {
        _appointmentServiceMock = new Mock<IAppointmentService>();
        _userManagerMock = MockUserManager<IdentityUser>();
        _controller = new AppointmentController(_appointmentServiceMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public async Task AddAppointmentAsync_ReturnsOkResult_WhenAppointmentIsAdded()
    {
        // Arrange
        var createAppointmentDto = new CreateAppointmentDTO
        {
            DoctorEmail = "doctor@example.com",
            PatientEmail = "patient@example.com"
        };

        var doctor = new IdentityUser { Email = "doctor@example.com", Id = "1" };
        var patient = new IdentityUser { Email = "patient@example.com", Id = "2" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(createAppointmentDto.DoctorEmail)).ReturnsAsync(doctor);
        _userManagerMock.Setup(x => x.FindByEmailAsync(createAppointmentDto.PatientEmail)).ReturnsAsync(patient);

        // Act
        var result = await _controller.AddAppointmentAsync(createAppointmentDto);

        // Assert
        var actionResult = Assert.IsType<OkResult>(result);
    }

   
    [Fact]
    public async Task CancelAppointmentAsync_ReturnsOkResult_WhenAppointmentIsCancelled()
    {
        // Act
        var result = await _controller.CancelAppointmentAsync(1);

        // Assert
        var actionResult = Assert.IsType<OkResult>(result);
    }

   

    [Fact]
    public async Task UpdateAppointmentAsync_ReturnsNoContentResult_WhenAppointmentIsUpdated()
    {
        // Arrange
        var createAppointmentDto = new CreateAppointmentDTO
        {
            DoctorEmail = "doctor@example.com",
            PatientEmail = "patient@example.com"
        };

        var doctor = new IdentityUser { Email = "doctor@example.com", Id = "1" };
        var patient = new IdentityUser { Email = "patient@example.com", Id = "2" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(createAppointmentDto.DoctorEmail)).ReturnsAsync(doctor);
        _userManagerMock.Setup(x => x.FindByEmailAsync(createAppointmentDto.PatientEmail)).ReturnsAsync(patient);

        // Act
        var result = await _controller.UpdateAppointmentAsync(1, createAppointmentDto);

        // Assert
        var actionResult = Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetAppointmentByPatientIdAsync_ReturnsOkResult_WithListOfAppointments()
    {
        // Arrange
        var appointments = new List<AppointmentDTO>
        {
            new AppointmentDTO { DoctorId = "1", PatientId = "2" }
        };

        _appointmentServiceMock.Setup(x => x.GetAppointmentByPatientIdAsync(It.IsAny<string>())).ReturnsAsync(appointments);

        // Act
        var result = await _controller.GetAppointmentByPatientIdAsync("2");

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<AppointmentDTO>>(actionResult.Value);
        Assert.Single(returnValue);
    }

    private Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
    {
        var store = new Mock<IUserStore<TUser>>();
        var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        return mgr;
    }
}
