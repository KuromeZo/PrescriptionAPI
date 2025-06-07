using System.ComponentModel.DataAnnotations;

namespace PrescriptionAPI.Models.DTOs;

public class CreatePrescriptionDto
{
    [Required]
    public PatientDto Patient { get; set; }
        
    [Required]
    public DoctorDto Doctor { get; set; }
        
    [Required]
    public DateTime Date { get; set; }
        
    [Required]
    public DateTime DueDate { get; set; }
        
    [Required]
    [MinLength(1)]
    [MaxLength(10)]
    public List<MedicamentDto> Medicaments { get; set; }
}
