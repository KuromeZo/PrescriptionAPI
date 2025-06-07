using PrescriptionAPI.Models.DTOs;

namespace PrescriptionAPI.Services.Interfaces;

public interface IPrescriptionService
{
    Task<int> CreatePrescriptionAsync(CreatePrescriptionDto createPrescriptionDto);
    Task<PatientDetailDto> GetPatientDetailsAsync(int patientId);
}