using HospitalSystem.Domain.Common;
using HospitalSystem.Domain.Enums;

namespace HospitalSystem.Domain.Entities;

public class AppointmentSlot : BaseEntity
{
    public int DoctorId { get; set; }

    public int PoliclinicRoomId { get; set; }

    public DateOnly SlotDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public SlotStatus Status { get; set; } = SlotStatus.Available;

    public Doctor Doctor { get; set; } = null!;

    public PoliclinicRoom PoliclinicRoom { get; set; } = null!;

    public Appointment? Appointment { get; set; }
}
