using HospitalSystem.Domain.Entities;
using HospitalSystem.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalSystem.Infrastructure.Persistence.Configurations;

public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
{
    public void Configure(EntityTypeBuilder<Specialty> builder)
    {
        builder.ToTable("Specialty");

        builder.HasKey(specialty => specialty.Id);

        builder.Property(specialty => specialty.Name).HasMaxLength(150);
        builder.Property(specialty => specialty.Description).HasMaxLength(1000);

        builder.HasData(ReferenceData.Specialties);
    }
}
