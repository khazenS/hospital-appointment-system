using HospitalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalSystem.Infrastructure.Persistence.Configurations;

public class DiagnosisConfiguration : IEntityTypeConfiguration<Diagnosis>
{
    public void Configure(EntityTypeBuilder<Diagnosis> builder)
    {
        builder.ToTable("Diagnosis");

        builder.HasKey(diagnosis => diagnosis.Id);

        builder.Property(diagnosis => diagnosis.Notes).HasMaxLength(2000);

        builder.HasOne(diagnosis => diagnosis.Appointment)
            .WithMany(appointment => appointment.Diagnoses)
            .HasForeignKey(diagnosis => diagnosis.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(diagnosis => diagnosis.ICDCode)
            .WithMany(icdCode => icdCode.Diagnoses)
            .HasForeignKey(diagnosis => diagnosis.ICDCodeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(diagnosis => diagnosis.AppointmentId).HasDatabaseName("IX_Diagnosis_AppointmentId");
        builder.HasIndex(diagnosis => diagnosis.ICDCodeId).HasDatabaseName("IX_Diagnosis_ICDCodeId");
    }
}
