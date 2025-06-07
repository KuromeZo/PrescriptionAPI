namespace PrescriptionAPI.Models.DTOs;

public class MedicamentDto
{
    public int IdMedicament { get; set; }
    public string Name { get; set; }
    public int Dose { get; set; }
    public string Details { get; set; }
}