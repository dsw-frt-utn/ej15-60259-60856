using System.Text.Json;
using Dsw2026Ej15.Domain;

namespace Dsw2026Ej15.Data;

public class PersistenceInMemory : IPersistence
{
    public List<Doctor> Doctors { get; private set; } = new List<Doctor>();
    public List<Speciality> Specialities { get; private set; } = new List<Speciality>();

    public PersistenceInMemory()
    {
        LoadSpecialities();
    }

    private void LoadSpecialities()
    {
        string path = "specialities.json";

        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            var specialities = JsonSerializer.Deserialize<List<Speciality>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true 
            });

            if (specialities != null)
            {
                Specialities = specialities;
            }
        }
    }

    public void AddDoctor(Doctor doctor)
    {
        Doctors.Add(doctor);
    }

    public Doctor? GetDoctorById(Guid id)
    {
        return Doctors.FirstOrDefault(d => d.Id == id);
    }

    public void InactivateDoctor(Guid id)
    {
        var doctor = GetDoctorById(id);
        if (doctor != null)
        {
            doctor.IsActive = false;
        }
    }
}