using HospitalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalSystem.Infrastructure.Persistence.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("Doctor");

        builder.HasKey(doctor => doctor.Id);

        builder.Property(doctor => doctor.Name).HasMaxLength(200);
        builder.Property(doctor => doctor.PhoneNumber).HasMaxLength(30);
        builder.Property(doctor => doctor.Email).HasMaxLength(200);
        builder.Property(doctor => doctor.IsActive).HasDefaultValue(true);

        builder.HasOne(doctor => doctor.PoliclinicRoom)
            .WithOne(room => room.Doctor)
            .HasForeignKey<Doctor>(doctor => doctor.PoliclinicRoomId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(doctor => doctor.Hospital)
            .WithMany(hospital => hospital.Doctors)
            .HasForeignKey(doctor => doctor.HospitalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(doctor => doctor.Specialty)
            .WithMany(specialty => specialty.Doctors)
            .HasForeignKey(doctor => doctor.SpecialtyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(doctor => doctor.SpecialtyId).HasDatabaseName("IX_Doctor_SpecialtyId");
        builder.HasIndex(doctor => doctor.HospitalId).HasDatabaseName("IX_Doctor_HospitalId");
    }
}
