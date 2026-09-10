using System.Globalization;
using HospitalSystem.Domain.Entities;
using HospitalSystem.Domain.Enums;
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

    case "add-patient":
        await AddPatientAsync(dbContext);
        break;

    default:
        Console.WriteLine("""
            Kullanım: dotnet run --project src/HospitalSystem.DbManager -- <komut>

              migrate       Bekleyen migration'ları uygular
              seed          Demo veriyi yükler (veri varsa atlar)
              reset         Veritabanını siler, yeniden kurar ve seed eder
              stats         Tablolardaki kayıt sayılarını yazar
              add-patient   Konsoldan bilgi alarak yeni hasta ekler

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

static async Task AddPatientAsync(HospitalDbContext dbContext)
{
    Console.WriteLine("Yeni hasta kaydı (boş bırakılan alanlar atlanır)");
    Console.WriteLine();

    var name = ReadRequiredText("Ad Soyad", 200);
    var phoneNumber = ReadRequiredText("Telefon", 30);

    var phoneNumberExists = await dbContext.Patients
        .AnyAsync(patient => patient.PhoneNumber == phoneNumber);

    if (phoneNumberExists)
    {
        Console.WriteLine();
        Console.WriteLine($"'{phoneNumber}' numarası zaten kayıtlı. İşlem iptal edildi.");
    }
    else
    {
        var email = ReadOptionalText("E-posta", 200);
        var address = ReadOptionalText("Adres", 500);
        var dateOfBirth = ReadOptionalDate("Doğum tarihi");
        var bloodType = ReadOptionalEnum<BloodType>("Kan grubu", PgEnums.BloodType);
        var gender = ReadOptionalEnum<Gender>("Cinsiyet", PgEnums.Gender);

        var patient = new Patient
        {
            Name = name,
            PhoneNumber = phoneNumber,
            Email = email,
            Address = address,
            DateOfBirth = dateOfBirth,
            BloodType = bloodType,
            Gender = gender
        };

        dbContext.Patients.Add(patient);
        await dbContext.SaveChangesAsync();

        Console.WriteLine();
        Console.WriteLine($"Hasta eklendi. Id = {patient.Id}");
    }
}

static string ReadRequiredText(string label, int maxLength)
{
    while (true)
    {
        Console.Write($"{label,-14}: ");
        var input = (Console.ReadLine() ?? string.Empty).Trim();

        if (input.Length == 0)
        {
            Console.WriteLine("  Bu alan zorunlu.");
        }
        else
        {
            if (input.Length > maxLength)
            {
                Console.WriteLine($"  En fazla {maxLength} karakter olabilir.");
            }
            else
            {
                return input;
            }
        }
    }
}

static string? ReadOptionalText(string label, int maxLength)
{
    while (true)
    {
        Console.Write($"{label,-14}: ");
        var input = (Console.ReadLine() ?? string.Empty).Trim();

        if (input.Length == 0)
        {
            return null;
        }
        else
        {
            if (input.Length > maxLength)
            {
                Console.WriteLine($"  En fazla {maxLength} karakter olabilir.");
            }
            else
            {
                return input;
            }
        }
    }
}

static DateTime? ReadOptionalDate(string label)
{
    while (true)
    {
        Console.Write($"{label,-14}: ");
        var input = (Console.ReadLine() ?? string.Empty).Trim();

        if (input.Length == 0)
        {
            return null;
        }
        else
        {
            var parsed = DateTime.TryParseExact(
                input,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var dateOfBirth);

            if (parsed)
            {
                // The column is timestamptz; Npgsql rejects DateTimeKind.Unspecified.
                return DateTime.SpecifyKind(dateOfBirth, DateTimeKind.Utc);
            }
            else
            {
                Console.WriteLine("  Biçim: yyyy-aa-gg (örnek: 1990-05-12)");
            }
        }
    }
}

static TEnum? ReadOptionalEnum<TEnum>(string label, LabelNameTranslator translator)
    where TEnum : struct, Enum
{
    var values = Enum.GetValues<TEnum>();

    Console.WriteLine();
    Console.WriteLine($"{label}:");

    for (var index = 0; index < values.Length; index++)
    {
        // Labels come from the translator so the console shows "A+" instead of "APositive".
        var displayName = translator.TranslateMemberName(values[index].ToString());
        Console.WriteLine($"  {index + 1}) {displayName}");
    }

    while (true)
    {
        Console.Write("Seçim (boş = atla): ");
        var input = (Console.ReadLine() ?? string.Empty).Trim();

        if (input.Length == 0)
        {
            return null;
        }
        else
        {
            var parsed = int.TryParse(input, out var choice);

            if (parsed && choice >= 1 && choice <= values.Length)
            {
                return values[choice - 1];
            }
            else
            {
                Console.WriteLine($"  1-{values.Length} arasında bir sayı gir.");
            }
        }
    }
}
