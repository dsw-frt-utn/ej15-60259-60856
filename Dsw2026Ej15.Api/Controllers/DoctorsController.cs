using Microsoft.AspNetCore.Mvc;
using Dsw2026Ej15.Data;
using Dsw2026Ej15.Domain;

namespace Dsw2026Ej15.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IPersistence _persistence;

    
    public DoctorsController(IPersistence persistence)
    {
        _persistence = persistence;
    }

   
    [HttpPost]
    public IActionResult CreateDoctor([FromBody] DoctorCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException("El nombre es requerido.");

        if (string.IsNullOrWhiteSpace(request.LicenseNumber))
            throw new ValidationException("El número de licencia es requerido.");

        var speciality = _persistence.Specialities.FirstOrDefault(s => s.Id == request.SpecialityId);
        if (speciality == null)
            throw new ValidationException("La especialidad indicada no existe.");

        var newDoctor = new Doctor
        {
            Name = request.Name,
            LicenseNumber = request.LicenseNumber,
            Speciality = speciality,
            IsActive = true
        };

        _persistence.AddDoctor(newDoctor);

        return Created($"/api/doctors/{newDoctor.Id}", newDoctor);
    }

    
    [HttpGet]
    public IActionResult GetActiveDoctors()
    {
        var activeDoctors = _persistence.Doctors.Where(d => d.IsActive).ToList();
        return Ok(activeDoctors);
    }

    
    [HttpGet("{id:guid}")]
    public IActionResult GetDoctorById(Guid id)
    {
        var doctor = _persistence.GetDoctorById(id);

        if (doctor == null || !doctor.IsActive)
            return NotFound(new { message = "Médico no encontrado o inactivo." });

        var response = new
        {
            Name = doctor.Name,
            LicenseNumber = doctor.LicenseNumber,
            SpecialityName = doctor.Speciality?.Name
        };

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteDoctor(Guid id)
    {
        var doctor = _persistence.GetDoctorById(id);

        if (doctor == null || !doctor.IsActive)
            return NotFound(new { message = "Médico no encontrado o ya está inactivo." });

        _persistence.InactivateDoctor(id);
        return NoContent(); 
    }
}


public class DoctorCreateRequest
{
    public string Name { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public Guid SpecialityId { get; set; }
}