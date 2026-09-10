using HospitalSystem.Domain.Common;

namespace HospitalSystem.Domain.Entities;

public class PrescriptionMedicine : BaseEntity
{
    public int PrescriptionId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Usage { get; set; } = null!;

    public Prescription Prescription { get; set; } = null!;
}
