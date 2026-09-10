using HospitalSystem.Domain.Common;
using HospitalSystem.Domain.Enums;

namespace HospitalSystem.Domain.Entities;

public class Doctor : BaseEntity
{
    public int PoliclinicId { get; set; }

    public int HospitalId { get; set; }

    public int SpecialtyId { get; set; }

    public string? Name { get; set; }

    public int? Age { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public DoctorTitle Title { get; set; }

    public DateOnly? HireDate { get; set; }

    public bool IsActive { get; set; } = true;

    public Policlinic Policlinic { get; set; } = null!;

    public Hospital Hospital { get; set; } = null!;

    public Specialty Specialty { get; set; } = null!;

    public List<DoctorSchedule> Schedules { get; set; } = new List<DoctorSchedule>();

    public List<AppointmentSlot> AppointmentSlots { get; set; } = new List<AppointmentSlot>();

    public List<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public List<MedicalHistory> RecordedMedicalHistories { get; set; } = new List<MedicalHistory>();
}
