using HospitalSystem.Domain.Entities;
using HospitalSystem.Domain.Enums;

namespace HospitalSystem.Domain.Services;

public static class SlotGenerator
{
    public static List<AppointmentSlot> Generate(
        Doctor doctor,
        List<DoctorSchedule> schedules,
        TimeSpan period,
        DateOnly fromDate,
        DateOnly toDate)
    {
        if (period <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(period), "Appointment period must be greater than zero.");
        }

        if (toDate < fromDate)
        {
            throw new ArgumentException("End date cannot be earlier than start date.", nameof(toDate));
        }

        var slots = new List<AppointmentSlot>();

        for (var date = fromDate; date <= toDate; date = date.AddDays(1))
        {
            foreach (var schedule in schedules)
            {
                if (AppliesTo(schedule, date))
                {
                    slots.AddRange(Slice(doctor, schedule, period, date));
                }
            }
        }

        return slots;
    }

    private static bool AppliesTo(DoctorSchedule schedule, DateOnly date)
    {
        if (schedule.IsActive && schedule.DayName == ToWeekDay(date.DayOfWeek))
        {
            // An empty ValidFrom/ValidTo means the template has no start or end boundary.
            var afterStart = schedule.ValidFrom == null || date >= schedule.ValidFrom;
            var beforeEnd = schedule.ValidTo == null || date <= schedule.ValidTo;

            if (afterStart && beforeEnd)
            {
                return true;
            }
        }

        return false;
    }

    private static List<AppointmentSlot> Slice(
        Doctor doctor,
        DoctorSchedule schedule,
        TimeSpan period,
        DateOnly date)
    {
        var slots = new List<AppointmentSlot>();
        var cursor = schedule.StartTime;

        while (true)
        {
            var end = cursor.Add(period);

            // TimeOnly.Add wraps around midnight, so "end <= cursor" means the day is over.
            // Leftover time that cannot hold a full period produces no slot.
            if (end > cursor && end <= schedule.EndTime)
            {
                slots.Add(new AppointmentSlot
                {
                    DoctorId = doctor.Id,
                    Doctor = doctor,
                    PoliclinicRoomId = schedule.PoliclinicRoomId,
                    SlotDate = date,
                    StartTime = cursor,
                    EndTime = end,
                    Status = SlotStatus.Available
                });

                cursor = end;
            }
            else
            {
                break;
            }
        }

        return slots;
    }

    private static WeekDay ToWeekDay(DayOfWeek day) => day switch
    {
        DayOfWeek.Monday => WeekDay.Monday,
        DayOfWeek.Tuesday => WeekDay.Tuesday,
        DayOfWeek.Wednesday => WeekDay.Wednesday,
        DayOfWeek.Thursday => WeekDay.Thursday,
        DayOfWeek.Friday => WeekDay.Friday,
        DayOfWeek.Saturday => WeekDay.Saturday,
        DayOfWeek.Sunday => WeekDay.Sunday,
        _ => throw new ArgumentOutOfRangeException(nameof(day))
    };
}
