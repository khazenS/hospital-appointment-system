using HospitalSystem.Domain.Common;

namespace HospitalSystem.Domain.Entities;

public class EmergencyContact : BaseEntity
{
    public int PatientId { get; set; }

    public string? Name { get; set; }

    public string? PhoneNumber { get; set; }

    public Patient Patient { get; set; } = null!;
}
