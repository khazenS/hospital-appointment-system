using HospitalSystem.Domain.Entities;
using HospitalSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalSystem.Infrastructure.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointment");

        builder.HasKey(appointment => appointment.Id);

        builder.Property(appointment => appointment.Status).HasDefaultValue(AppointmentStatus.Waiting);

        builder.HasOne(appointment => appointment.Patient)
            .WithMany(patient => patient.Appointments)
            .HasForeignKey(appointment => appointment.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        // One-to-one on SlotId is the real protection against double booking. Checking
        // availability in application code first would still lose to a race condition.
        builder.HasOne(appointment => appointment.Slot)
            .WithOne(slot => slot.Appointment)
            .HasForeignKey<Appointment>(appointment => appointment.SlotId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(appointment => appointment.PatientId).HasDatabaseName("IX_Appointment_PatientId");
    }
}
