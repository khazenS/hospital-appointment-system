using HospitalSystem.Domain.Common;
using HospitalSystem.Domain.Enums;

namespace HospitalSystem.Domain.Entities;

public class LabTest : BaseEntity
{
    public int AppointmentId { get; set; }

    public string? TestName { get; set; }

    public string? TestType { get; set; }

    public DateTime? RequestedDate { get; set; }

    public DateTime? ResultDate { get; set; }

    public string? Result { get; set; }

    public LabTestStatus Status { get; set; } = LabTestStatus.Requested;

    public string? Notes { get; set; }

    public Appointment Appointment { get; set; } = null!;
}
