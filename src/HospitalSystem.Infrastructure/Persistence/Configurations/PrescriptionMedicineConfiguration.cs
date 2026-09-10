using HospitalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalSystem.Infrastructure.Persistence.Configurations;

public class PrescriptionMedicineConfiguration : IEntityTypeConfiguration<PrescriptionMedicine>
{
    public void Configure(EntityTypeBuilder<PrescriptionMedicine> builder)
    {
        // The drawn schema spelled this table "PresriptionMedicine"; the typo was corrected here.
        builder.ToTable("PrescriptionMedicine");

        builder.HasKey(medicine => medicine.Id);

        builder.Property(medicine => medicine.Name).HasMaxLength(300).IsRequired();
        builder.Property(medicine => medicine.Description).HasMaxLength(1000);
        builder.Property(medicine => medicine.Usage).HasMaxLength(500).IsRequired();

        builder.HasOne(medicine => medicine.Prescription)
            .WithMany(prescription => prescription.Medicines)
            .HasForeignKey(medicine => medicine.PrescriptionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(medicine => medicine.PrescriptionId)
            .HasDatabaseName("IX_PrescriptionMedicine_PrescriptionId");
    }
}
