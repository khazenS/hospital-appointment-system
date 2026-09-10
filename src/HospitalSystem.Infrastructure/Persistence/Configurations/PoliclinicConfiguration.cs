using HospitalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalSystem.Infrastructure.Persistence.Configurations;

public class PoliclinicConfiguration : IEntityTypeConfiguration<Policlinic>
{
    public void Configure(EntityTypeBuilder<Policlinic> builder)
    {
        builder.ToTable("Policlinic");

        builder.HasKey(policlinic => policlinic.Id);

        builder.Property(policlinic => policlinic.Name).HasMaxLength(150);
        builder.Property(policlinic => policlinic.Description).HasMaxLength(1000);
        builder.Property(policlinic => policlinic.PhoneNumber).HasMaxLength(30);
        builder.Property(policlinic => policlinic.Location).HasMaxLength(200);
        builder.Property(policlinic => policlinic.AppointmentTimePeriod).HasColumnType("interval");
        builder.Property(policlinic => policlinic.IsActive).HasDefaultValue(true);

        builder.HasIndex(policlinic => policlinic.PhoneNumber)
            .IsUnique()
            .HasDatabaseName("UX_Policlinic_PhoneNumber");

        builder.HasOne(policlinic => policlinic.Hospital)
            .WithMany(hospital => hospital.Policlinics)
            .HasForeignKey(policlinic => policlinic.HospitalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
