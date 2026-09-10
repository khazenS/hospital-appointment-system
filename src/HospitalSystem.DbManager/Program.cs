using HospitalSystem.Infrastructure.Persistence;
using HospitalSystem.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

var command = args.FirstOrDefault() ?? "help";

var connectionString =
    Environment.GetEnvironmentVariable("HOSPITALDB_CONNECTION")
    ?? HospitalDbContextFactory.DefaultConnectionString;

await using var dbContext = new HospitalDbContext(HospitalDbContextFactory.BuildOptions(connectionString));

switch (command)
{
    case "migrate":
        await MigrateAsync(dbContext);
        break;

    case "seed":
        await SeedAsync(dbContext);
        break;

    case "reset":
        Console.WriteLine("Veritabanı siliniyor...");
        await dbContext.Database.EnsureDeletedAsync();
        await MigrateAsync(dbContext);
        await SeedAsync(dbContext);
        break;

    case "stats":
        await PrintStatsAsync(dbContext);
        break;

    default:
        Console.WriteLine("""
            Kullanım: dotnet run --project src/HospitalSystem.DbManager -- <komut>

              migrate   Bekleyen migration'ları uygular
              seed      Demo veriyi yükler (veri varsa atlar)
              reset     Veritabanını siler, yeniden kurar ve seed eder
              stats     Tablolardaki kayıt sayılarını yazar

            Bağlantı dizesi HOSPITALDB_CONNECTION ortam değişkeninden okunur.
            """);
        break;
}

return;

static async Task MigrateAsync(HospitalDbContext dbContext)
{
    var pendingMigrations = (await dbContext.Database.GetPendingMigrationsAsync()).ToList();

    if (pendingMigrations.Count > 0)
    {
        Console.WriteLine($"{pendingMigrations.Count} migration uygulanıyor: {string.Join(", ", pendingMigrations)}");
        await dbContext.Database.MigrateAsync();
        Console.WriteLine("Migration tamamlandı.");
    }
    else
    {
        Console.WriteLine("Bekleyen migration yok.");
    }
}

static async Task SeedAsync(HospitalDbContext dbContext)
{
    var seeded = await new DemoDataSeeder(dbContext).SeedAsync();

    if (seeded)
    {
        Console.WriteLine("Demo veri yüklendi.");
    }
    else
    {
        Console.WriteLine("Veritabanında zaten veri var, seed atlandı.");
    }
}

static async Task PrintStatsAsync(HospitalDbContext dbContext)
{
    var rows = new List<(string Label, int Count)>
    {
        ("Hospital", await dbContext.Hospitals.CountAsync()),
        ("Policlinic", await dbContext.Policlinics.CountAsync()),
        ("PoliclinicRoom", await dbContext.PoliclinicRooms.CountAsync()),
        ("Specialty", await dbContext.Specialties.CountAsync()),
        ("Doctor", await dbContext.Doctors.CountAsync()),
        ("DoctorSchedule", await dbContext.DoctorSchedules.CountAsync()),
        ("Patient", await dbContext.Patients.CountAsync()),
        ("EmergencyContact", await dbContext.EmergencyContacts.CountAsync()),
        ("AppointmentSlot", await dbContext.AppointmentSlots.CountAsync()),
        ("Appointment", await dbContext.Appointments.CountAsync()),
        ("MedicalHistory", await dbContext.MedicalHistories.CountAsync()),
        ("Diagnosis", await dbContext.Diagnoses.CountAsync()),
        ("ICDCode", await dbContext.ICDCodes.CountAsync()),
        ("Prescription", await dbContext.Prescriptions.CountAsync()),
        ("PrescriptionMedicine", await dbContext.PrescriptionMedicines.CountAsync()),
        ("LabTest", await dbContext.LabTests.CountAsync())
    };

    foreach (var (label, count) in rows)
    {
        Console.WriteLine($"{label,-22} {count,6}");
    }
}
