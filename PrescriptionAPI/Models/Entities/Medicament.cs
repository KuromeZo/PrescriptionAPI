using System.ComponentModel.DataAnnotations;

namespace PrescriptionAPI.Models.Entities;

public class Medicament
{
    public int IdMedicament { get; set; }
        
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
        
    [StringLength(100)]
    public string Description { get; set; }
        
    [StringLength(100)]
    public string Type { get; set; }
        
    public virtual ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; } = new List<PrescriptionMedicament>();
}