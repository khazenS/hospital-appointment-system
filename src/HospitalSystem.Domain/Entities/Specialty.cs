using HospitalSystem.Domain.Common;

namespace HospitalSystem.Domain.Entities;

public class Specialty : BaseEntity
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public List<Doctor> Doctors { get; set; } = new List<Doctor>();
}
