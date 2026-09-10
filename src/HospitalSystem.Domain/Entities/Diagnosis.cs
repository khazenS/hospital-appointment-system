using HospitalSystem.Domain.Common;

namespace HospitalSystem.Domain.Entities;

public class Diagnosis : BaseEntity
{
    public int AppointmentId { get; set; }

    public int ICDCodeId { get; set; }

    public DateTime? DiagnosisDate { get; set; }

    public string? Notes { get; set; }

    public Appointment Appointment { get; set; } = null!;

    public ICDCode ICDCode { get; set; } = null!;
}
