using HospitalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalSystem.Infrastructure.Persistence.Configurations;

public class HospitalConfiguration : IEntityTypeConfiguration<Hospital>
{
    public void Configure(EntityTypeBuilder<Hospital> builder)
    {
        builder.ToTable("Hospital");

        builder.HasKey(hospital => hospital.Id);

        builder.Property(hospital => hospital.Address).HasMaxLength(500);
        builder.Property(hospital => hospital.City).HasMaxLength(100);
        builder.Property(hospital => hospital.PhoneNumber).HasMaxLength(30);

        builder.HasIndex(hospital => hospital.PhoneNumber)
            .IsUnique()
            .HasDatabaseName("UX_Hospital_PhoneNumber");

        builder.HasIndex(hospital => hospital.City).HasDatabaseName("IX_Hospital_City");
    }
}
