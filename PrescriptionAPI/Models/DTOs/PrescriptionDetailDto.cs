namespace PrescriptionAPI.Models.DTOs;

public class PrescriptionDetailDto
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<MedicamentDetailDto> Medicaments { get; set; }
    public DoctorDetailDto Doctor { get; set; }
}