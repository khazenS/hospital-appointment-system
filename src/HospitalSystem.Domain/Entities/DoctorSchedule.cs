using HospitalSystem.Domain.Common;
using HospitalSystem.Domain.Enums;

namespace HospitalSystem.Domain.Entities;

public class DoctorSchedule : BaseEntity
{
    public int DoctorId { get; set; }

    public WeekDay DayName { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public bool IsActive { get; set; } = true;

    public DateOnly? ValidFrom { get; set; }

    public DateOnly? ValidTo { get; set; }

    public Doctor Doctor { get; set; } = null!;
}
