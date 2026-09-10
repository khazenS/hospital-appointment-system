using HospitalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalSystem.Infrastructure.Persistence.Configurations;

public class PoliclinicRoomConfiguration : IEntityTypeConfiguration<PoliclinicRoom>
{
    public void Configure(EntityTypeBuilder<PoliclinicRoom> builder)
    {
        builder.ToTable("PoliclinicRoom");

        builder.HasKey(room => room.Id);

        builder.Property(room => room.Location).HasMaxLength(200);
        builder.Property(room => room.IsActive).HasDefaultValue(true);

        builder.HasOne(room => room.Policlinic)
            .WithMany(policlinic => policlinic.Rooms)
            .HasForeignKey(room => room.PoliclinicId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
