using HospitalSystem.Domain.Common;
using HospitalSystem.Domain.Enums;

namespace HospitalSystem.Domain.Entities;

public class Appointment : BaseEntity
{
    public int PatientId { get; set; }

    public int SlotId { get; set; }

    public AppointmentStatus Status { get; set; } = AppointmentStatus.Waiting;

    public Patient Patient { get; set; } = null!;

    public AppointmentSlot Slot { get; set; } = null!;

    public List<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();

    public List<LabTest> LabTests { get; set; } = new List<LabTest>();

    public List<MedicalHistory> MedicalHistories { get; set; } = new List<MedicalHistory>();

    public Prescription? Prescription { get; set; }
}
