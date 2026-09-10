using System.Reflection;
using HospitalSystem.Domain.Common;
using HospitalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Infrastructure.Persistence;

public class HospitalDbContext : DbContext
{
    public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
    {
    }

    public DbSet<Hospital> Hospitals => this.Set<Hospital>();
    public DbSet<Policlinic> Policlinics => this.Set<Policlinic>();
    public DbSet<PoliclinicRoom> PoliclinicRooms => this.Set<PoliclinicRoom>();
    public DbSet<Specialty> Specialties => this.Set<Specialty>();
    public DbSet<Doctor> Doctors => this.Set<Doctor>();
    public DbSet<DoctorSchedule> DoctorSchedules => this.Set<DoctorSchedule>();
    public DbSet<Patient> Patients => this.Set<Patient>();
    public DbSet<EmergencyContact> EmergencyContacts => this.Set<EmergencyContact>();
    public DbSet<AppointmentSlot> AppointmentSlots => this.Set<AppointmentSlot>();
    public DbSet<Appointment> Appointments => this.Set<Appointment>();
    public DbSet<MedicalHistory> MedicalHistories => this.Set<MedicalHistory>();
    public DbSet<Diagnosis> Diagnoses => this.Set<Diagnosis>();
    public DbSet<ICDCode> ICDCodes => this.Set<ICDCode>();
    public DbSet<Prescription> Prescriptions => this.Set<Prescription>();
    public DbSet<PrescriptionMedicine> PrescriptionMedicines => this.Set<PrescriptionMedicine>();
    public DbSet<LabTest> LabTests => this.Set<LabTest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        PgEnums.Register(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        this.ApplyAuditTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.ApplyAuditTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditTimestamps()
    {
        // Columns are timestamptz, so the value written must be UTC.
        var now = DateTime.UtcNow;

        foreach (var entry in this.ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                    break;

                case EntityState.Modified:
                    entry.Property(e => e.CreatedAt).IsModified = false;
                    entry.Entity.UpdatedAt = now;
                    break;
            }
        }
    }
}
