using Microsoft.EntityFrameworkCore;
using PrescriptionAPI.Data;
using PrescriptionAPI.Models.DTOs;
using PrescriptionAPI.Models.Entities;
using PrescriptionAPI.Services.Interfaces;

namespace PrescriptionAPI.Services;

public class PrescriptionService : IPrescriptionService
    {
        private readonly PrescriptionDbContext _context;

        public PrescriptionService(PrescriptionDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreatePrescriptionAsync(CreatePrescriptionDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                // validate due date
                if (dto.DueDate < dto.Date)
                {
                    throw new ArgumentException("DueDate must be greater than or equal to Date");
                }

                // validate medications count
                if (dto.Medicaments.Count > 10)
                {
                    throw new ArgumentException("A prescription can include a maximum of 10 medications");
                }

                // if exist
                var medicamentIds = dto.Medicaments.Select(m => m.IdMedicament).ToList();
                var existingMedicaments = await _context.Medicaments
                    .Where(m => medicamentIds.Contains(m.IdMedicament))
                    .Select(m => m.IdMedicament)
                    .ToListAsync();

                var missingMedicaments = medicamentIds.Except(existingMedicaments).ToList();
                if (missingMedicaments.Any())
                {
                    throw new ArgumentException($"Medications with IDs [{string.Join(", ", missingMedicaments)}] do not exist");
                }

                // Find(create) patient
                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.FirstName == dto.Patient.FirstName && 
                                            p.LastName == dto.Patient.LastName && 
                                            p.Birthdate.Date == dto.Patient.Birthdate.Date);

                if (patient == null)
                {
                    patient = new Patient
                    {
                        FirstName = dto.Patient.FirstName,
                        LastName = dto.Patient.LastName,
                        Birthdate = dto.Patient.Birthdate
                    };
                    _context.Patients.Add(patient);
                    await _context.SaveChangesAsync();
                }

                // if doctor exists
                var doctor = await _context.Doctors.FindAsync(dto.Doctor.IdDoctor);
                if (doctor == null)
                {
                    throw new ArgumentException($"Doctor with ID {dto.Doctor.IdDoctor} does not exist");
                }

                // new prescription
                var prescription = new Prescription
                {
                    Date = dto.Date,
                    DueDate = dto.DueDate,
                    IdPatient = patient.IdPatient,
                    IdDoctor = dto.Doctor.IdDoctor
                };

                _context.Prescriptions.Add(prescription);
                await _context.SaveChangesAsync();

                // Add medications
                foreach (var medicamentDto in dto.Medicaments)
                {
                    var prescriptionMedicament = new PrescriptionMedicament
                    {
                        IdPrescription = prescription.IdPrescription,
                        IdMedicament = medicamentDto.IdMedicament,
                        Dose = medicamentDto.Dose,
                        Details = medicamentDto.Details
                    };
                    _context.PrescriptionMedicaments.Add(prescriptionMedicament);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return prescription.IdPrescription;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<PatientDetailDto> GetPatientDetailsAsync(int patientId)
        {
            var patient = await _context.Patients
                .Include(p => p.Prescriptions.OrderBy(pr => pr.DueDate))
                    .ThenInclude(pr => pr.Doctor)
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.PrescriptionMedicaments)
                        .ThenInclude(pm => pm.Medicament)
                .FirstOrDefaultAsync(p => p.IdPatient == patientId);

            if (patient == null)
            {
                throw new ArgumentException($"Patient with ID {patientId} not found");
            }

            return new PatientDetailDto
            {
                IdPatient = patient.IdPatient,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Birthdate = patient.Birthdate,
                Prescriptions = patient.Prescriptions.Select(p => new PrescriptionDetailDto
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new DoctorDetailDto
                    {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName,
                        LastName = p.Doctor.LastName
                    },
                    Medicaments = p.PrescriptionMedicaments.Select(pm => new MedicamentDetailDto
                    {
                        IdMedicament = pm.Medicament.IdMedicament,
                        Name = pm.Medicament.Name,
                        Dose = pm.Dose,
                        Description = pm.Medicament.Description,
                        Details = pm.Details
                    }).ToList()
                }).ToList()
            };
        }
    }