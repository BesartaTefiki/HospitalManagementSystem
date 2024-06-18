using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Repositories.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HospitalManagementSystem.Tests.Repositories
{
    public class AppointmentRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly AppDbContext _context;
        private readonly AppointmentRepository _repository;

        public AppointmentRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(_options);
            _repository = new AppointmentRepository(_context);
        }

        [Fact]
        public async Task AddAppointmentAsync_ShouldAddAppointment()
        {
            // Arrange
            var appointment = new Appointment
            {
                PatientId = "P1",
                DoctorId = "D1",
                AppointmentDate = DateTime.Now
            };

            // Act
            await _repository.AddAppointmentAsync(appointment);
            var addedAppointment = await _context.Appointments.FirstOrDefaultAsync(a => a.PatientId == "P1");

            // Assert
            Assert.NotNull(addedAppointment);
            Assert.Equal(appointment.PatientId, addedAppointment.PatientId);
        }

        [Fact]
        public async Task CancelAppointmentAsync_ShouldRemoveAppointment()
        {
            // Arrange
            var appointment = new Appointment
            {
                PatientId = "P1",
                DoctorId = "D1",
                AppointmentDate = DateTime.Now
            };
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();

            // Act
            await _repository.CancelAppointmentAsync(appointment.Id);
            var removedAppointment = await _context.Appointments.FindAsync(appointment.Id);

            // Assert
            Assert.Null(removedAppointment);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_ShouldReturnAppointment()
        {
            // Arrange
            var appointment = new Appointment
            {
                PatientId = "P1",
                DoctorId = "D1",
                AppointmentDate = DateTime.Now
            };
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAppointmentByIdAsync(appointment.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(appointment.Id, result.Id);
        }


        [Fact]
        public async Task GetAppointmentsAsync_ShouldReturnAllAppointments()
        {
            // Arrange
            var appointments = new List<Appointment>
    {
        new Appointment
        {
            PatientId = "P1",
            DoctorId = "D1",
            AppointmentDate = DateTime.Now,
            Patient = new IdentityUser { Email = "patient1@example.com" },
            Doctor = new IdentityUser { Email = "doctor1@example.com" }
        },
        new Appointment
        {
            PatientId = "P2",
            DoctorId = "D2",
            AppointmentDate = DateTime.Now,
            Patient = new IdentityUser { Email = "patient2@example.com" },
            Doctor = new IdentityUser { Email = "doctor2@example.com" }
        }
    };
            await _context.Appointments.AddRangeAsync(appointments);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAppointmentsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("patient1@example.com", result.ElementAt(0).Patient.Email);
            Assert.Equal("doctor1@example.com", result.ElementAt(0).Doctor.Email);
            Assert.Equal("patient2@example.com", result.ElementAt(1).Patient.Email);
            Assert.Equal("doctor2@example.com", result.ElementAt(1).Doctor.Email);
        }

        [Fact]
        public async Task UpdateAppointmentAsync_ShouldUpdateAppointment()
        {
            // Arrange
            var appointment = new Appointment
            {
                PatientId = "P1",
                DoctorId = "D1",
                AppointmentDate = DateTime.Now
            };
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();

            var updatedAppointment = new Appointment
            {
                PatientId = "P2",
                DoctorId = "D2",
                AppointmentDate = DateTime.Now.AddDays(1),
                IsCancelled = true
            };

            // Act
            await _repository.UpdateAppointmentAsync(updatedAppointment, appointment.Id);
            var result = await _context.Appointments.FindAsync(appointment.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("P2", result.PatientId);
            Assert.Equal("D2", result.DoctorId);
            Assert.Equal(updatedAppointment.AppointmentDate, result.AppointmentDate);
            Assert.True(result.IsCancelled);
        }
    }
}
