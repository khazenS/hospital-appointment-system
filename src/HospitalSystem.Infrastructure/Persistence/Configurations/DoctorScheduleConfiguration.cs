using HospitalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalSystem.Infrastructure.Persistence.Configurations;

public class DoctorScheduleConfiguration : IEntityTypeConfiguration<DoctorSchedule>
{
    public void Configure(EntityTypeBuilder<DoctorSchedule> builder)
    {
        builder.ToTable("DoctorSchedule", table =>
        {
            table.HasCheckConstraint("CK_DoctorSchedule_TimeRange", "\"EndTime\" > \"StartTime\"");
            table.HasCheckConstraint(
                "CK_DoctorSchedule_ValidRange",
                "\"ValidTo\" IS NULL OR \"ValidFrom\" IS NULL OR \"ValidTo\" >= \"ValidFrom\"");
        });

        builder.HasKey(schedule => schedule.Id);

        builder.Property(schedule => schedule.IsActive).HasDefaultValue(true);

        builder.HasOne(schedule => schedule.Doctor)
            .WithMany(doctor => doctor.Schedules)
            .HasForeignKey(schedule => schedule.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(schedule => schedule.PoliclinicRoom)
            .WithMany(room => room.Schedules)
            .HasForeignKey(schedule => schedule.PoliclinicRoomId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(schedule => new { schedule.DoctorId, schedule.DayName })
            .HasDatabaseName("IX_DoctorSchedule_DoctorId_DayName");

        builder.HasIndex(schedule => schedule.PoliclinicRoomId)
            .HasDatabaseName("IX_DoctorSchedule_PoliclinicRoomId");
    }
}
