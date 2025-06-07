using Microsoft.EntityFrameworkCore;
using PrescriptionAPI.Data;
using PrescriptionAPI.Models.DTOs;
using PrescriptionAPI.Models.Entities;
using PrescriptionAPI.Services;
using Xunit;

namespace PrescriptionAPI.Tests;

public class PrescriptionServiceTests : IDisposable
    {
        private readonly PrescriptionDbContext _context;
        private readonly PrescriptionService _service;

        public PrescriptionServiceTests()
        {
            var options = new DbContextOptionsBuilder<PrescriptionDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new PrescriptionDbContext(options);
            _service = new PrescriptionService(_context);

            SeedTestData();
        }

        private void SeedTestData()
        {
            var doctor = new Doctor
            {
                IdDoctor = 1,
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@example.com"
            };

            var medicament = new Medicament
            {
                IdMedicament = 1,
                Name = "Aspirin",
                Description = "Pain reliever",
                Type = "Tablet"
            };

            _context.Doctors.Add(doctor);
            _context.Medicaments.Add(medicament);
            _context.SaveChanges();
        }

        [Fact]
        public async Task CreatePrescriptionAsync_WithValidData_ShouldCreatePrescription()
        {
            var dto = new CreatePrescriptionDto
            {
                Patient = new PatientDto
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    Birthdate = new DateTime(1990, 1, 1)
                },
                Doctor = new DoctorDto
                {
                    IdDoctor = 1,
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "john.smith@example.com"
                },
                Date = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7),
                Medicaments = new List<MedicamentDto>
                {
                    new MedicamentDto
                    {
                        IdMedicament = 1,
                        Dose = 2,
                        Details = "Take twice daily"
                    }
                }
            };

            var result = await _service.CreatePrescriptionAsync(dto);

            Assert.True(result > 0);
            var prescription = await _context.Prescriptions.FindAsync(result);
            Assert.NotNull(prescription);
        }

        [Fact]
        public async Task CreatePrescriptionAsync_WithInvalidDueDate_ShouldThrowException()
        {
            var dto = new CreatePrescriptionDto
            {
                Patient = new PatientDto
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    Birthdate = new DateTime(1990, 1, 1)
                },
                Doctor = new DoctorDto
                {
                    IdDoctor = 1,
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "john.smith@example.com"
                },
                Date = DateTime.Now,
                DueDate = DateTime.Now.AddDays(-1), // Invalid: due date before prescription date
                Medicaments = new List<MedicamentDto>
                {
                    new MedicamentDto
                    {
                        IdMedicament = 1,
                        Dose = 2,
                        Details = "Take twice daily"
                    }
                }
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePrescriptionAsync(dto));
        }

        [Fact]
        public async Task CreatePrescriptionAsync_WithTooManyMedicaments_ShouldThrowException()
        {
            var medicaments = new List<MedicamentDto>();
            for (int i = 0; i < 11; i++)
            {
                medicaments.Add(new MedicamentDto
                {
                    IdMedicament = 1,
                    Dose = 1,
                    Details = $"Medicament {i}"
                });
            }

            var dto = new CreatePrescriptionDto
            {
                Patient = new PatientDto
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    Birthdate = new DateTime(1990, 1, 1)
                },
                Doctor = new DoctorDto
                {
                    IdDoctor = 1,
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "john.smith@example.com"
                },
                Date = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7),
                Medicaments = medicaments
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePrescriptionAsync(dto));
        }

        [Fact]
        public async Task CreatePrescriptionAsync_WithNonExistentMedicament_ShouldThrowException()
        {
            var dto = new CreatePrescriptionDto
            {
                Patient = new PatientDto
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    Birthdate = new DateTime(1990, 1, 1)
                },
                Doctor = new DoctorDto
                {
                    IdDoctor = 1,
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "john.smith@example.com"
                },
                Date = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7),
                Medicaments = new List<MedicamentDto>
                {
                    new MedicamentDto
                    {
                        IdMedicament = 999, // Non-existent
                        Dose = 2,
                        Details = "Take twice daily"
                    }
                }
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePrescriptionAsync(dto));
        }

        [Fact]
        public async Task GetPatientDetailsAsync_WithValidPatientId_ShouldReturnPatientDetails()
        {
            var patient = new Patient
            {
                FirstName = "John",
                LastName = "Doe",
                Birthdate = new DateTime(1980, 5, 15)
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            var result = await _service.GetPatientDetailsAsync(patient.IdPatient);

            Assert.NotNull(result);
            Assert.Equal(patient.FirstName, result.FirstName);
            Assert.Equal(patient.LastName, result.LastName);
        }

        [Fact]
        public async Task GetPatientDetailsAsync_WithInvalidPatientId_ShouldThrowException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _service.GetPatientDetailsAsync(999));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }