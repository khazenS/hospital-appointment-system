using HospitalSystem.Domain.Entities;
using HospitalSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalSystem.Infrastructure.Persistence.Configurations;

public class AppointmentSlotConfiguration : IEntityTypeConfiguration<AppointmentSlot>
{
    public void Configure(EntityTypeBuilder<AppointmentSlot> builder)
    {
        builder.ToTable("AppointmentSlot", table =>
            table.HasCheckConstraint("CK_AppointmentSlot_TimeRange", "\"EndTime\" > \"StartTime\""));

        builder.HasKey(slot => slot.Id);

        builder.Property(slot => slot.Status).HasDefaultValue(SlotStatus.Available);

        builder.HasOne(slot => slot.Doctor)
            .WithMany(doctor => doctor.AppointmentSlots)
            .HasForeignKey(slot => slot.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(slot => slot.PoliclinicRoom)
            .WithMany(room => room.AppointmentSlots)
            .HasForeignKey(slot => slot.PoliclinicRoomId)
            .OnDelete(DeleteBehavior.Restrict);

        // Makes slot generation safe to re-run: a second pass over the same dates
        // cannot insert duplicates.
        builder.HasIndex(slot => new { slot.DoctorId, slot.SlotDate, slot.StartTime })
            .IsUnique()
            .HasDatabaseName("UX_AppointmentSlot_Doctor_Date_Start");

        // A room can host only one doctor at a time. The old Doctor-Room one-to-one
        // constraint used to guarantee this implicitly; now the index has to.
        builder.HasIndex(slot => new { slot.PoliclinicRoomId, slot.SlotDate, slot.StartTime })
            .IsUnique()
            .HasDatabaseName("UX_AppointmentSlot_Room_Date_Start");

        builder.HasIndex(slot => new { slot.SlotDate, slot.Status })
            .HasDatabaseName("IX_AppointmentSlot_SlotDate_Status");
    }
}
