using HospitalSystem.Domain.Entities;
using HospitalSystem.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalSystem.Infrastructure.Persistence.Configurations;

public class ICDCodeConfiguration : IEntityTypeConfiguration<ICDCode>
{
    public void Configure(EntityTypeBuilder<ICDCode> builder)
    {
        builder.ToTable("ICDCode");

        builder.HasKey(icdCode => icdCode.Id);

        builder.Property(icdCode => icdCode.Code).HasMaxLength(20).IsRequired();
        builder.Property(icdCode => icdCode.Title).HasMaxLength(500).IsRequired();
        builder.Property(icdCode => icdCode.Description).HasMaxLength(2000);

        builder.HasIndex(icdCode => icdCode.Code)
            .IsUnique()
            .HasDatabaseName("UX_ICDCode_Code");

        builder.HasData(ReferenceData.ICDCodes);
    }
}
