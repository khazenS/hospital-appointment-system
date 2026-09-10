using HospitalSystem.Domain.Common;

namespace HospitalSystem.Domain.Entities;

public class Policlinic : BaseEntity
{
    public int HospitalId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Location { get; set; }

    // Slot length for this policlinic. SlotGenerator reads it to size every AppointmentSlot.
    public TimeSpan? AppointmentTimePeriod { get; set; }

    public bool IsActive { get; set; } = true;

    public Hospital Hospital { get; set; } = null!;

    public List<PoliclinicRoom> Rooms { get; set; } = new List<PoliclinicRoom>();

    public List<Doctor> Doctors { get; set; } = new List<Doctor>();
}
