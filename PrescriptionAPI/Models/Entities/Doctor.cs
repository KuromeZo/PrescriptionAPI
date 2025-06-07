using System.ComponentModel.DataAnnotations;

namespace PrescriptionAPI.Models.Entities;

public class Doctor
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
        
    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}