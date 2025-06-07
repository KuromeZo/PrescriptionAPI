namespace PrescriptionAPI.Models.DTOs;

public class MedicamentDetailDto
{
    public int IdMedicament { get; set; }
    public string Name { get; set; }
    public int Dose { get; set; }
    public string Description { get; set; }
    public string Details { get; set; }
}