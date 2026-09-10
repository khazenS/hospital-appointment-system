using HospitalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalSystem.Infrastructure.Persistence.Configurations;

public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
{
    public void Configure(EntityTypeBuilder<Prescription> builder)
    {
        builder.ToTable("Prescription");

        builder.HasKey(prescription => prescription.Id);

        builder.Property(prescription => prescription.Notes).HasMaxLength(2000);

        builder.HasOne(prescription => prescription.Doctor)
            .WithMany(doctor => doctor.Prescriptions)
            .HasForeignKey(prescription => prescription.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(prescription => prescription.Patient)
            .WithMany(patient => patient.Prescriptions)
            .HasForeignKey(prescription => prescription.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(prescription => prescription.Appointment)
            .WithOne(appointment => appointment.Prescription)
            .HasForeignKey<Prescription>(prescription => prescription.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(prescription => prescription.PatientId).HasDatabaseName("IX_Prescription_PatientId");
        builder.HasIndex(prescription => prescription.DoctorId).HasDatabaseName("IX_Prescription_DoctorId");
    }
}
