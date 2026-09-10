using HospitalSystem.Domain.Common;

namespace HospitalSystem.Domain.Entities;

public class Prescription : BaseEntity
{
    public int DoctorId { get; set; }

    public int PatientId { get; set; }

    public int AppointmentId { get; set; }

    public DateTime? PrescriptionDate { get; set; }

    public string? Notes { get; set; }

    public Doctor Doctor { get; set; } = null!;

    public Patient Patient { get; set; } = null!;

    public Appointment Appointment { get; set; } = null!;

    public List<PrescriptionMedicine> Medicines { get; set; } = new List<PrescriptionMedicine>();
}
