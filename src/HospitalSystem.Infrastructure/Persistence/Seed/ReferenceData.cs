using HospitalSystem.Domain.Entities;

namespace HospitalSystem.Infrastructure.Persistence.Seed;

// Reference data baked into the migration through HasData. It has to be deterministic:
// ids are assigned by hand and timestamps use the fixed SeededAt value instead of
// DateTime.UtcNow. Otherwise every "dotnet ef migrations add" emits a pointless
// UpdateData migration.
public static class ReferenceData
{
    public static readonly DateTime SeededAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static readonly Specialty[] Specialties = new Specialty[]
    {
        NewSpecialty(1, "Kardiyoloji", "Kalp ve dolaşım sistemi hastalıkları"),
        NewSpecialty(2, "Dahiliye", "İç hastalıkları"),
        NewSpecialty(3, "Nöroloji", "Sinir sistemi hastalıkları"),
        NewSpecialty(4, "Ortopedi ve Travmatoloji", "Kas-iskelet sistemi hastalıkları ve kırıklar"),
        NewSpecialty(5, "Göz Hastalıkları", "Göz ve görme bozuklukları"),
        NewSpecialty(6, "Kulak Burun Boğaz", "KBB hastalıkları"),
        NewSpecialty(7, "Çocuk Sağlığı ve Hastalıkları", "Pediatri"),
        NewSpecialty(8, "Genel Cerrahi", "Cerrahi girişim gerektiren hastalıklar"),
        NewSpecialty(9, "Dermatoloji", "Deri hastalıkları"),
        NewSpecialty(10, "Kadın Hastalıkları ve Doğum", "Jinekoloji ve obstetri")
    };

    public static readonly ICDCode[] ICDCodes = new ICDCode[]
    {
        NewIcdCode(1, "I10", "Esansiyel (primer) hipertansiyon"),
        NewIcdCode(2, "I20.9", "Angina pektoris, tanımlanmamış"),
        NewIcdCode(3, "E11", "Tip 2 diabetes mellitus"),
        NewIcdCode(4, "E78.5", "Hiperlipidemi, tanımlanmamış"),
        NewIcdCode(5, "J06.9", "Akut üst solunum yolu enfeksiyonu, tanımlanmamış"),
        NewIcdCode(6, "J45", "Astım"),
        NewIcdCode(7, "K21.0", "Gastroözofageal reflü hastalığı, özofajitli"),
        NewIcdCode(8, "A09", "Enfeksiyöz kaynaklı olduğu varsayılan gastroenterit ve kolit"),
        NewIcdCode(9, "M54.5", "Bel ağrısı"),
        NewIcdCode(10, "S52.5", "Radius alt uç kırığı"),
        NewIcdCode(11, "G43.9", "Migren, tanımlanmamış"),
        NewIcdCode(12, "R51", "Baş ağrısı"),
        NewIcdCode(13, "N39.0", "Üriner sistem enfeksiyonu, yeri belirtilmemiş"),
        NewIcdCode(14, "H52.1", "Miyopi"),
        NewIcdCode(15, "L20.9", "Atopik dermatit, tanımlanmamış")
    };

    private static Specialty NewSpecialty(int id, string name, string description) => new Specialty
    {
        Id = id,
        Name = name,
        Description = description,
        CreatedAt = SeededAt,
        UpdatedAt = SeededAt
    };

    private static ICDCode NewIcdCode(int id, string code, string title) => new ICDCode
    {
        Id = id,
        Code = code,
        Title = title,
        CreatedAt = SeededAt,
        UpdatedAt = SeededAt
    };
}
