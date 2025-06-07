using System.ComponentModel.DataAnnotations;

namespace PrescriptionAPI.Models.Entities;

public class Patient
{
    public int IdPatient { get; set; }
        
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }
        
    [Required]
    [StringLength(100)]
    public string LastName { get; set; }
        
    public DateTime Birthdate { get; set; }
        
    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}