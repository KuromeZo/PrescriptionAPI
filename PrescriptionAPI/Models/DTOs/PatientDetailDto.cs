﻿namespace PrescriptionAPI.Models.DTOs;

public class PatientDetailDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }
    public List<PrescriptionDetailDto> Prescriptions { get; set; }
}