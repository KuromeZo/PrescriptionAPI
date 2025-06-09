using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrescriptionAPI.Models.DTOs;
using PrescriptionAPI.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PrescriptionController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;

    public PrescriptionController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePrescription([FromBody] CreatePrescriptionDto createPrescriptionDto)
    {
        try
        {
            var prescriptionId = await _prescriptionService.CreatePrescriptionAsync(createPrescriptionDto);
            return CreatedAtAction(nameof(CreatePrescription), new { id = prescriptionId }, new { Id = prescriptionId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "An error occurred while creating the prescription" });
        }
    }

    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetPatientDetails(int patientId)
    {
        try
        {
            var patientDetails = await _prescriptionService.GetPatientDetailsAsync(patientId);
            return Ok(patientDetails);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "An error occurred while retrieving patient details" });
        }
    }
}