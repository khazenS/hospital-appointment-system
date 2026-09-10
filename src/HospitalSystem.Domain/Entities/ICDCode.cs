using HospitalSystem.Domain.Common;

namespace HospitalSystem.Domain.Entities;

public class ICDCode : BaseEntity
{
    public string Code { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public List<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();
}
