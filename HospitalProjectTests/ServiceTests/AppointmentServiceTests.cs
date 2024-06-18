using AutoMapper;
using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Repositories.Interfaces;
using HospitalManagementSystem.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace HospitalManagementSystem.Tests.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly IMapper _mapper;
        private readonly AppointmentService _appointmentService;

        public AppointmentServiceTests()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateAppointmentDTO, Appointment>();
                cfg.CreateMap<Appointment, AppointmentDTO>();
            });
            _mapper = config.CreateMapper();

            _appointmentService = new AppointmentService(_appointmentRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task AddAppointmentAsync_ShouldCallRepository()
        {
            // Arrange
            var createAppointmentDto = new CreateAppointmentDTO
            {
                PatientId = "P1",
                DoctorId = "D1",
                AppointmentDate = DateTime.Now
            };

            // Act
            await _appointmentService.AddAppointmentAsync(createAppointmentDto);

            // Assert
            _appointmentRepositoryMock.Verify(repo => repo.AddAppointmentAsync(It.IsAny<Appointment>()), Times.Once);
        }

        [Fact]
        public async Task CancelAppointmentAsync_ShouldCallRepository()
        {
            // Arrange
            var appointmentId = 1;

            // Act
            await _appointmentService.CancelAppointmentAsync(appointmentId);

            // Assert
            _appointmentRepositoryMock.Verify(repo => repo.CancelAppointmentAsync(appointmentId), Times.Once);
        }
        [Fact]
        public async Task GetAppointmentsAsync_ShouldReturnAppointmentDTOs()
        {
            // Arrange
            var appointments = new List<Appointment>
    {
        new Appointment
        {
            Id = 1,
            PatientId = "P1",
            DoctorId = "D1",
            AppointmentDate = DateTime.Now,
            Patient = new IdentityUser { Email = "patient1@example.com" },
            Doctor = new IdentityUser { Email = "doctor1@example.com" }
        },
        new Appointment
        {
            Id = 2,
            PatientId = "P2",
            DoctorId = "D2",
            AppointmentDate = DateTime.Now,
            Patient = new IdentityUser { Email = "patient2@example.com" },
            Doctor = new IdentityUser { Email = "doctor2@example.com" }
        }
    };
            _appointmentRepositoryMock.Setup(repo => repo.GetAppointmentsAsync()).ReturnsAsync(appointments);

            // Act
            var result = await _appointmentService.GetAppointmentsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, item => Assert.IsType<AppointmentDTO>(item));
            Assert.Equal("patient1@example.com", result.ElementAt(0).PatientEmail);
            Assert.Equal("doctor1@example.com", result.ElementAt(0).DoctorEmail);
            Assert.Equal("patient2@example.com", result.ElementAt(1).PatientEmail);
            Assert.Equal("doctor2@example.com", result.ElementAt(1).DoctorEmail);
        }



        [Fact]
        public async Task GetAppointmentByIdAsync_ShouldReturnAppointmentDTO()
        {
            // Arrange
            var appointment = new Appointment
            {
                Id = 1,
                PatientId = "P1",
                DoctorId = "D1",
                AppointmentDate = DateTime.Now
            };
            _appointmentRepositoryMock.Setup(repo => repo.GetAppointmentByIdAsync(appointment.Id)).ReturnsAsync(appointment);

            // Act
            var result = await _appointmentService.GetAppointmentByIdAsync(appointment.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<AppointmentDTO>(result);
            Assert.Equal(appointment.Id, result.Id);
        }

        [Fact]
        public async Task UpdateAppointmentAsync_ShouldCallRepository()
        {
            // Arrange
            var appointment = new Appointment
            {
                Id = 1,
                PatientId = "P1",
                DoctorId = "D1",
                AppointmentDate = DateTime.Now
            };
            var appointmentId = 1;

            // Act
            await _appointmentService.UpdateAppointmentAsync(appointment, appointmentId);

            // Assert
            _appointmentRepositoryMock.Verify(repo => repo.UpdateAppointmentAsync(appointment, appointmentId), Times.Once);
        }
    }
}
