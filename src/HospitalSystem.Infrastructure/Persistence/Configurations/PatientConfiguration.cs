using HospitalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalSystem.Infrastructure.Persistence.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("Patient");

        builder.HasKey(patient => patient.Id);

        builder.Property(patient => patient.Name).HasMaxLength(200);
        builder.Property(patient => patient.PhoneNumber).HasMaxLength(30);
        builder.Property(patient => patient.Address).HasMaxLength(500);
        builder.Property(patient => patient.Email).HasMaxLength(200);

        builder.HasIndex(patient => patient.PhoneNumber)
            .IsUnique()
            .HasDatabaseName("UX_Patient_PhoneNumber");

        builder.HasIndex(patient => patient.Name).HasDatabaseName("IX_Patient_Name");
    }
}
