using System.ComponentModel.DataAnnotations;

namespace PrescriptionAPI.Models.Entities;

public class PrescriptionMedicament
{
    public int IdMedicament { get; set; }
    public virtual Medicament Medicament { get; set; }
        
    public int IdPrescription { get; set; }
    public virtual Prescription Prescription { get; set; }
        
    public int Dose { get; set; }
        
    [StringLength(100)]
    public string Details { get; set; }
}