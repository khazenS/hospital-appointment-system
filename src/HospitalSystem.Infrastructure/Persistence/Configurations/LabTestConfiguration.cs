using HospitalSystem.Domain.Entities;
using HospitalSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalSystem.Infrastructure.Persistence.Configurations;

public class LabTestConfiguration : IEntityTypeConfiguration<LabTest>
{
    public void Configure(EntityTypeBuilder<LabTest> builder)
    {
        builder.ToTable("LabTest");

        builder.HasKey(labTest => labTest.Id);

        builder.Property(labTest => labTest.TestName).HasMaxLength(300);
        builder.Property(labTest => labTest.TestType).HasMaxLength(150);
        builder.Property(labTest => labTest.Result).HasMaxLength(2000);
        builder.Property(labTest => labTest.Notes).HasMaxLength(2000);
        builder.Property(labTest => labTest.Status).HasDefaultValue(LabTestStatus.Requested);

        builder.HasOne(labTest => labTest.Appointment)
            .WithMany(appointment => appointment.LabTests)
            .HasForeignKey(labTest => labTest.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(labTest => labTest.AppointmentId).HasDatabaseName("IX_LabTest_AppointmentId");
        builder.HasIndex(labTest => labTest.Status).HasDatabaseName("IX_LabTest_Status");
    }
}
