using HospitalSystem.Domain.Common;

namespace HospitalSystem.Domain.Entities;

public class PoliclinicRoom : BaseEntity
{
    public int PoliclinicId { get; set; }

    public string? Location { get; set; }

    public bool IsActive { get; set; } = true;

    public Policlinic Policlinic { get; set; } = null!;

    public Doctor? Doctor { get; set; }

    public List<AppointmentSlot> AppointmentSlots { get; set; } = new List<AppointmentSlot>();
}
