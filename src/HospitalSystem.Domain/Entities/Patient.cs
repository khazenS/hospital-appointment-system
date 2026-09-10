using HospitalSystem.Domain.Common;
using HospitalSystem.Domain.Enums;

namespace HospitalSystem.Domain.Entities;

public class Patient : BaseEntity
{
    public string? Name { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public BloodType? BloodType { get; set; }

    public Gender? Gender { get; set; }

    public List<EmergencyContact> EmergencyContacts { get; set; } = new List<EmergencyContact>();

    public List<Appointment> Appointments { get; set; } = new List<Appointment>();

    public List<MedicalHistory> MedicalHistories { get; set; } = new List<MedicalHistory>();

    public List<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
