using HospitalSystem.Domain.Common;
using HospitalSystem.Domain.Enums;

namespace HospitalSystem.Domain.Entities;

public class Hospital : BaseEntity
{
    public string? Address { get; set; }

    public string? City { get; set; }

    public string? PhoneNumber { get; set; }

    public HospitalType Type { get; set; }

    public int? Capacity { get; set; }

    public DateTime? EstablishDate { get; set; }

    public List<Policlinic> Policlinics { get; set; } = new List<Policlinic>();

    public List<Doctor> Doctors { get; set; } = new List<Doctor>();
}
