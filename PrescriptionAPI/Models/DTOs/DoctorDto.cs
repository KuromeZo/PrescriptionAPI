using System.ComponentModel.DataAnnotations;

namespace PrescriptionAPI.Models.DTOs;

public class DoctorDto
{
    public int IdDoctor { get; set; }
        
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }
        
    [Required]
    [StringLength(100)]
    public string LastName { get; set; }
        
    [Required]
    [StringLength(100)]
    public string Email { get; set; }
}