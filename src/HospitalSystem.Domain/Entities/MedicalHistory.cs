using HospitalSystem.Domain.Common;
using HospitalSystem.Domain.Enums;

namespace HospitalSystem.Domain.Entities;

public class MedicalHistory : BaseEntity
{
    public int PatientId { get; set; }

    public MedicalHistoryType Type { get; set; }

    public string? Description { get; set; }

    public DateTime? DiagnosedDate { get; set; }

    public int RecordedByDoctorId { get; set; }

    public int AppointmentId { get; set; }

    public Patient Patient { get; set; } = null!;

    public Doctor RecordedByDoctor { get; set; } = null!;

    public Appointment Appointment { get; set; } = null!;
}
