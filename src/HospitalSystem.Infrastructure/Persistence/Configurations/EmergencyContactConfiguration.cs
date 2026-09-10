using HospitalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalSystem.Infrastructure.Persistence.Configurations;

public class EmergencyContactConfiguration : IEntityTypeConfiguration<EmergencyContact>
{
    public void Configure(EntityTypeBuilder<EmergencyContact> builder)
    {
        builder.ToTable("EmergencyContact");

        builder.HasKey(contact => contact.Id);

        builder.Property(contact => contact.Name).HasMaxLength(200);
        builder.Property(contact => contact.PhoneNumber).HasMaxLength(30);

        builder.HasIndex(contact => contact.PhoneNumber)
            .IsUnique()
            .HasDatabaseName("UX_EmergencyContact_PhoneNumber");

        builder.HasOne(contact => contact.Patient)
            .WithMany(patient => patient.EmergencyContacts)
            .HasForeignKey(contact => contact.PatientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
