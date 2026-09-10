using HospitalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalSystem.Infrastructure.Persistence.Configurations;

public class MedicalHistoryConfiguration : IEntityTypeConfiguration<MedicalHistory>
{
    public void Configure(EntityTypeBuilder<MedicalHistory> builder)
    {
        builder.ToTable("MedicalHistory");

        builder.HasKey(history => history.Id);

        builder.Property(history => history.Description).HasMaxLength(2000);

        // All three foreign keys use Restrict on purpose. Patient and Appointment both
        // reach this table, so a cascade on either one would create multiple cascade paths
        // and PostgreSQL would refuse to create the table.
        builder.HasOne(history => history.Patient)
            .WithMany(patient => patient.MedicalHistories)
            .HasForeignKey(history => history.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(history => history.RecordedByDoctor)
            .WithMany(doctor => doctor.RecordedMedicalHistories)
            .HasForeignKey(history => history.RecordedByDoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(history => history.Appointment)
            .WithMany(appointment => appointment.MedicalHistories)
            .HasForeignKey(history => history.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(history => new { history.PatientId, history.DiagnosedDate })
            .HasDatabaseName("IX_MedicalHistory_PatientId_DiagnosedDate");

        builder.HasIndex(history => history.AppointmentId).HasDatabaseName("IX_MedicalHistory_AppointmentId");
    }
}
