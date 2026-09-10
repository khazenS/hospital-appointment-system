using HospitalSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace HospitalSystem.Infrastructure.Persistence;

// Some labels in the schema are not valid C# identifiers ("A+", "Doç. Dr.", "Being-diagnosed"),
// so the CLR-member-to-label mapping lives here in a dictionary instead of on the enums themselves.
// Register (EF model) and MapAll (Npgsql driver) must stay in sync; if they drift apart,
// reads and writes fail at runtime with a type error.
public static class PgEnums
{
    public static readonly LabelNameTranslator BloodType = new("blood_type", new()
    {
        [nameof(Domain.Enums.BloodType.APositive)] = "A+",
        [nameof(Domain.Enums.BloodType.ANegative)] = "A-",
        [nameof(Domain.Enums.BloodType.BPositive)] = "B+",
        [nameof(Domain.Enums.BloodType.BNegative)] = "B-",
        [nameof(Domain.Enums.BloodType.ABPositive)] = "AB+",
        [nameof(Domain.Enums.BloodType.ABNegative)] = "AB-",
        [nameof(Domain.Enums.BloodType.ZeroPositive)] = "0+",
        [nameof(Domain.Enums.BloodType.ZeroNegative)] = "0-"
    });

    public static readonly LabelNameTranslator Gender = new("gender", new()
    {
        [nameof(Domain.Enums.Gender.Male)] = "Male",
        [nameof(Domain.Enums.Gender.Female)] = "Female"
    });

    public static readonly LabelNameTranslator HospitalType = new("hospital_type", new()
    {
        [nameof(Domain.Enums.HospitalType.Government)] = "Government",
        [nameof(Domain.Enums.HospitalType.Primary)] = "Primary",
        [nameof(Domain.Enums.HospitalType.University)] = "University"
    });

    public static readonly LabelNameTranslator DoctorTitle = new("title", new()
    {
        [nameof(Domain.Enums.DoctorTitle.Dr)] = "Dr.",
        [nameof(Domain.Enums.DoctorTitle.UzmDr)] = "Uzm. Dr.",
        [nameof(Domain.Enums.DoctorTitle.DocDr)] = "Doç. Dr.",
        [nameof(Domain.Enums.DoctorTitle.ProfDr)] = "Prof. Dr.",
        [nameof(Domain.Enums.DoctorTitle.OpDr)] = "Op. Dr.",
        [nameof(Domain.Enums.DoctorTitle.AsistDr)] = "Asist. Dr."
    });

    public static readonly LabelNameTranslator AppointmentStatus = new("appointment_status", new()
    {
        [nameof(Domain.Enums.AppointmentStatus.Waiting)] = "Waiting",
        [nameof(Domain.Enums.AppointmentStatus.Cancelled)] = "Cancelled",
        [nameof(Domain.Enums.AppointmentStatus.BeingDiagnosed)] = "Being-diagnosed",
        [nameof(Domain.Enums.AppointmentStatus.Diagnosed)] = "Diagnosed"
    });

    public static readonly LabelNameTranslator SlotStatus = new("slot_status", new()
    {
        [nameof(Domain.Enums.SlotStatus.Available)] = "Available",
        [nameof(Domain.Enums.SlotStatus.Booked)] = "Booked",
        [nameof(Domain.Enums.SlotStatus.Blocked)] = "Blocked"
    });

    public static readonly LabelNameTranslator WeekDay = new("day_of_week", new()
    {
        [nameof(Domain.Enums.WeekDay.Monday)] = "Pazartesi",
        [nameof(Domain.Enums.WeekDay.Tuesday)] = "Salı",
        [nameof(Domain.Enums.WeekDay.Wednesday)] = "Çarşamba",
        [nameof(Domain.Enums.WeekDay.Thursday)] = "Perşembe",
        [nameof(Domain.Enums.WeekDay.Friday)] = "Cuma",
        [nameof(Domain.Enums.WeekDay.Saturday)] = "Cumartesi",
        [nameof(Domain.Enums.WeekDay.Sunday)] = "Pazar"
    });

    public static readonly LabelNameTranslator MedicalHistoryType = new("medical_history_type", new()
    {
        [nameof(Domain.Enums.MedicalHistoryType.Chronic)] = "chronic",
        [nameof(Domain.Enums.MedicalHistoryType.Surgical)] = "surgical",
        [nameof(Domain.Enums.MedicalHistoryType.Allergic)] = "allergic"
    });

    public static readonly LabelNameTranslator LabTestStatus = new("lab_test_status", new()
    {
        [nameof(Domain.Enums.LabTestStatus.Requested)] = "Requested",
        [nameof(Domain.Enums.LabTestStatus.Waiting)] = "Waiting",
        [nameof(Domain.Enums.LabTestStatus.Completed)] = "Completed"
    });

    public static void Register(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<BloodType>(null, BloodType.PgTypeName, BloodType);
        modelBuilder.HasPostgresEnum<Gender>(null, Gender.PgTypeName, Gender);
        modelBuilder.HasPostgresEnum<HospitalType>(null, HospitalType.PgTypeName, HospitalType);
        modelBuilder.HasPostgresEnum<DoctorTitle>(null, DoctorTitle.PgTypeName, DoctorTitle);
        modelBuilder.HasPostgresEnum<AppointmentStatus>(null, AppointmentStatus.PgTypeName, AppointmentStatus);
        modelBuilder.HasPostgresEnum<SlotStatus>(null, SlotStatus.PgTypeName, SlotStatus);
        modelBuilder.HasPostgresEnum<WeekDay>(null, WeekDay.PgTypeName, WeekDay);
        modelBuilder.HasPostgresEnum<MedicalHistoryType>(null, MedicalHistoryType.PgTypeName, MedicalHistoryType);
        modelBuilder.HasPostgresEnum<LabTestStatus>(null, LabTestStatus.PgTypeName, LabTestStatus);
    }

    public static void MapAll(NpgsqlDataSourceBuilder builder)
    {
        builder.MapEnum<BloodType>(BloodType.PgTypeName, BloodType);
        builder.MapEnum<Gender>(Gender.PgTypeName, Gender);
        builder.MapEnum<HospitalType>(HospitalType.PgTypeName, HospitalType);
        builder.MapEnum<DoctorTitle>(DoctorTitle.PgTypeName, DoctorTitle);
        builder.MapEnum<AppointmentStatus>(AppointmentStatus.PgTypeName, AppointmentStatus);
        builder.MapEnum<SlotStatus>(SlotStatus.PgTypeName, SlotStatus);
        builder.MapEnum<WeekDay>(WeekDay.PgTypeName, WeekDay);
        builder.MapEnum<MedicalHistoryType>(MedicalHistoryType.PgTypeName, MedicalHistoryType);
        builder.MapEnum<LabTestStatus>(LabTestStatus.PgTypeName, LabTestStatus);
    }
}

public sealed class LabelNameTranslator : INpgsqlNameTranslator
{
    private readonly Dictionary<string, string> _labels;

    public LabelNameTranslator(string pgTypeName, Dictionary<string, string> labels)
    {
        this.PgTypeName = pgTypeName;
        this._labels = labels;
    }

    public string PgTypeName { get; }

    public string TranslateTypeName(string clrName) => this.PgTypeName;

    public string TranslateMemberName(string clrName)
    {
        if (this._labels.TryGetValue(clrName, out var label))
        {
            return label;
        }

        // Failing loudly beats silently writing a wrong label into the database.
        throw new InvalidOperationException(
            $"Member '{clrName}' is not mapped for enum '{this.PgTypeName}' in PgEnums.");
    }
}
