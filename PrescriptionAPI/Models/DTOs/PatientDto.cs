using System.ComponentModel.DataAnnotations;

namespace PrescriptionAPI.Models.DTOs;

public class PatientDto
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }
        
    [Required]
    [StringLength(100)]
    public string LastName { get; set; }
        
    public DateTime Birthdate { get; set; }
}