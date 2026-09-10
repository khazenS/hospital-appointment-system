using HospitalSystem.Domain.Entities;
using HospitalSystem.Domain.Enums;
using HospitalSystem.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Infrastructure.Persistence.Seed;

public class DemoDataSeeder
{
    // Fixed seed keeps every run producing the same demo data.
    private readonly Random _random = new Random(42);
    private readonly HospitalDbContext _dbContext;

    public DemoDataSeeder(HospitalDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<bool> SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await this._dbContext.Hospitals.AnyAsync(cancellationToken))
        {
            return false;
        }

        var hospital = await this.SeedHospitalAsync(cancellationToken);
        var doctors = await this.SeedPoliclinicsAndDoctorsAsync(hospital, cancellationToken);
        await this.SeedSchedulesAsync(doctors, cancellationToken);
        var slots = await this.SeedSlotsAsync(doctors, cancellationToken);
        var patients = await this.SeedPatientsAsync(cancellationToken);
        await this.SeedAppointmentsAsync(patients, slots, cancellationToken);

        return true;
    }

    private async Task<Hospital> SeedHospitalAsync(CancellationToken cancellationToken)
    {
        var hospital = new Hospital
        {
            Address = "Üniversite Mah. Sağlık Cad. No:1",
            City = "İstanbul",
            PhoneNumber = "+90 212 000 00 00",
            Type = HospitalType.University,
            Capacity = 850,
            EstablishDate = new DateTime(1998, 9, 1, 0, 0, 0, DateTimeKind.Utc)
        };

        this._dbContext.Hospitals.Add(hospital);
        await this._dbContext.SaveChangesAsync(cancellationToken);
        return hospital;
    }

    private async Task<List<Doctor>> SeedPoliclinicsAndDoctorsAsync(
        Hospital hospital,
        CancellationToken cancellationToken)
    {
        var definitions = new List<(string Name, int SpecialtyId, int PeriodMinutes)>
        {
            ("Kardiyoloji Polikliniği", 1, 20),
            ("Dahiliye Polikliniği", 2, 15),
            ("Nöroloji Polikliniği", 3, 20),
            ("Çocuk Polikliniği", 7, 15)
        };

        var doctorNames = new List<(string Name, DoctorTitle Title)>
        {
            ("Ayşe Yıldırım", DoctorTitle.ProfDr),
            ("Mehmet Aksoy", DoctorTitle.UzmDr),
            ("Zeynep Demir", DoctorTitle.DocDr),
            ("Emre Kaya", DoctorTitle.UzmDr),
            ("Elif Şahin", DoctorTitle.ProfDr),
            ("Burak Çelik", DoctorTitle.Dr),
            ("Selin Arslan", DoctorTitle.UzmDr),
            ("Kerem Doğan", DoctorTitle.AsistDr)
        };

        var doctors = new List<Doctor>();
        var nameIndex = 0;

        foreach (var (name, specialtyId, periodMinutes) in definitions)
        {
            var policlinic = new Policlinic
            {
                Hospital = hospital,
                Name = name,
                Description = $"{name} muayene ve takip birimi",
                PhoneNumber = $"+90 212 000 0{definitions.Count + nameIndex:00}",
                Location = $"{nameIndex + 1}. Kat",
                AppointmentTimePeriod = TimeSpan.FromMinutes(periodMinutes),
                IsActive = true
            };

            this._dbContext.Policlinics.Add(policlinic);

            for (var roomNumber = 1; roomNumber <= 2; roomNumber++)
            {
                var room = new PoliclinicRoom
                {
                    Policlinic = policlinic,
                    Location = $"{nameIndex + 1}. Kat - Oda {roomNumber}",
                    IsActive = true
                };

                this._dbContext.PoliclinicRooms.Add(room);
            }

            // Doctors belong to the policlinic, not to a room. Which room they sit in
            // is decided per shift in SeedSchedulesAsync.
            for (var doctorNumber = 1; doctorNumber <= 2; doctorNumber++)
            {
                var (doctorName, title) = doctorNames[nameIndex];
                var doctor = new Doctor
                {
                    Policlinic = policlinic,
                    Hospital = hospital,
                    SpecialtyId = specialtyId,
                    Name = doctorName,
                    Age = 34 + this._random.Next(0, 25),
                    PhoneNumber = $"+90 532 000 {nameIndex:00} {doctorNumber:00}",
                    Email = $"doktor{nameIndex + 1}@hastane.example",
                    Title = title,
                    HireDate = new DateOnly(2010 + this._random.Next(0, 14), 1 + this._random.Next(0, 12), 1),
                    IsActive = true
                };

                this._dbContext.Doctors.Add(doctor);
                doctors.Add(doctor);
                nameIndex++;
            }
        }

        await this._dbContext.SaveChangesAsync(cancellationToken);
        return doctors;
    }

    private async Task SeedSchedulesAsync(List<Doctor> doctors, CancellationToken cancellationToken)
    {
        var weekdays = new List<WeekDay>
        {
            WeekDay.Monday, WeekDay.Tuesday, WeekDay.Wednesday, WeekDay.Thursday, WeekDay.Friday
        };

        // Two shifts a day, leaving the lunch hour uncovered.
        var shifts = new List<(TimeOnly StartTime, TimeOnly EndTime)>
        {
            (new TimeOnly(9, 0), new TimeOnly(12, 0)),
            (new TimeOnly(13, 0), new TimeOnly(17, 0))
        };

        var rooms = await this._dbContext.PoliclinicRooms
            .OrderBy(room => room.Id)
            .ToListAsync(cancellationToken);

        var roomsByPoliclinic = rooms
            .GroupBy(room => room.PoliclinicId)
            .ToDictionary(group => group.Key, group => group.ToList());

        var doctorCountByPoliclinic = new Dictionary<int, int>();

        foreach (var doctor in doctors)
        {
            var policlinicRooms = roomsByPoliclinic[doctor.PoliclinicId];

            doctorCountByPoliclinic.TryGetValue(doctor.PoliclinicId, out var doctorIndex);
            doctorCountByPoliclinic[doctor.PoliclinicId] = doctorIndex + 1;

            foreach (var day in weekdays)
            {
                for (var shiftIndex = 0; shiftIndex < shifts.Count; shiftIndex++)
                {
                    // Shifting the room by the doctor's position keeps two doctors of the
                    // same policlinic out of the same room in the same shift.
                    var room = policlinicRooms[(doctorIndex + shiftIndex) % policlinicRooms.Count];
                    var (startTime, endTime) = shifts[shiftIndex];

                    this._dbContext.DoctorSchedules.Add(
                        CreateSchedule(doctor, room, day, startTime, endTime));
                }
            }
        }

        await this._dbContext.SaveChangesAsync(cancellationToken);
    }

    private static DoctorSchedule CreateSchedule(
        Doctor doctor,
        PoliclinicRoom room,
        WeekDay day,
        TimeOnly startTime,
        TimeOnly endTime)
    {
        return new DoctorSchedule
        {
            Doctor = doctor,
            PoliclinicRoom = room,
            DayName = day,
            StartTime = startTime,
            EndTime = endTime,
            IsActive = true,
            ValidFrom = new DateOnly(2026, 1, 1)
        };
    }

    private async Task<List<AppointmentSlot>> SeedSlotsAsync(
        List<Doctor> doctors,
        CancellationToken cancellationToken)
    {
        // A doctor's slot length comes from their policlinic.
        var periods = await this._dbContext.Doctors
            .Select(doctor => new
            {
                doctor.Id,
                Period = doctor.Policlinic.AppointmentTimePeriod
            })
            .ToDictionaryAsync(entry => entry.Id, entry => entry.Period, cancellationToken);

        var schedules = await this._dbContext.DoctorSchedules.ToListAsync(cancellationToken);

        var fromDate = new DateOnly(2026, 9, 14);
        var toDate = fromDate.AddDays(13);

        var slots = new List<AppointmentSlot>();

        foreach (var doctor in doctors)
        {
            var period = periods[doctor.Id] ?? TimeSpan.FromMinutes(15);
            var doctorSchedules = schedules
                .Where(schedule => schedule.DoctorId == doctor.Id)
                .ToList();

            slots.AddRange(SlotGenerator.Generate(doctor, doctorSchedules, period, fromDate, toDate));
        }

        this._dbContext.AppointmentSlots.AddRange(slots);
        await this._dbContext.SaveChangesAsync(cancellationToken);
        return slots;
    }

    private async Task<List<Patient>> SeedPatientsAsync(CancellationToken cancellationToken)
    {
        var definitions = new List<(string Name, Gender Gender, BloodType BloodType, int BirthYear)>
        {
            ("Ali Vural", Gender.Male, BloodType.APositive, 1978),
            ("Fatma Öztürk", Gender.Female, BloodType.ZeroPositive, 1985),
            ("Hasan Korkmaz", Gender.Male, BloodType.BNegative, 1962),
            ("Merve Aydın", Gender.Female, BloodType.ABPositive, 1994),
            ("Okan Şimşek", Gender.Male, BloodType.ANegative, 2001),
            ("Deniz Polat", Gender.Female, BloodType.ZeroNegative, 1989),
            ("Kaan Erdem", Gender.Male, BloodType.BPositive, 1970),
            ("Sude Yalçın", Gender.Female, BloodType.ABNegative, 2015)
        };

        var patients = new List<Patient>();
        var index = 0;

        foreach (var (name, gender, bloodType, birthYear) in definitions)
        {
            var patient = new Patient
            {
                Name = name,
                PhoneNumber = $"+90 555 000 {index:00} {index:00}",
                Address = $"{index + 1}. Sokak No:{10 + index}, İstanbul",
                Email = $"hasta{index + 1}@example.com",
                DateOfBirth = new DateTime(
                    birthYear,
                    1 + this._random.Next(0, 12),
                    1 + this._random.Next(0, 28),
                    0, 0, 0, DateTimeKind.Utc),
                BloodType = bloodType,
                Gender = gender
            };

            patient.EmergencyContacts.Add(new EmergencyContact
            {
                Name = $"{name.Split(' ')[1]} ailesi - acil",
                PhoneNumber = $"+90 555 111 {index:00} {index:00}"
            });

            this._dbContext.Patients.Add(patient);
            patients.Add(patient);
            index++;
        }

        await this._dbContext.SaveChangesAsync(cancellationToken);
        return patients;
    }

    private async Task SeedAppointmentsAsync(
        List<Patient> patients,
        List<AppointmentSlot> slots,
        CancellationToken cancellationToken)
    {
        var today = new DateOnly(2026, 9, 20);

        var pastSlots = slots
            .Where(slot => slot.SlotDate < today)
            .OrderBy(slot => slot.Id)
            .ToList();

        var futureSlots = slots
            .Where(slot => slot.SlotDate >= today)
            .OrderBy(slot => slot.Id)
            .ToList();

        var completedSlots = PickEvenlySpaced(pastSlots, 24);
        var upcomingSlots = PickEvenlySpaced(futureSlots, 12);

        for (var index = 0; index < completedSlots.Count; index++)
        {
            var slot = completedSlots[index];
            var patient = patients[index % patients.Count];
            var appointment = this.BookSlot(slot, patient, AppointmentStatus.Diagnosed);

            this.AddClinicalRecords(appointment, slot, patient);
        }

        for (var index = 0; index < upcomingSlots.Count; index++)
        {
            var slot = upcomingSlots[index];
            var patient = patients[(index + 3) % patients.Count];
            this.BookSlot(slot, patient, AppointmentStatus.Waiting);
        }

        await this._dbContext.SaveChangesAsync(cancellationToken);
    }

    private Appointment BookSlot(AppointmentSlot slot, Patient patient, AppointmentStatus status)
    {
        slot.Status = SlotStatus.Booked;

        var appointment = new Appointment
        {
            Patient = patient,
            Slot = slot,
            Status = status
        };

        this._dbContext.Appointments.Add(appointment);
        return appointment;
    }

    private void AddClinicalRecords(Appointment appointment, AppointmentSlot slot, Patient patient)
    {
        var examinedAt = slot.SlotDate.ToDateTime(slot.StartTime, DateTimeKind.Utc);
        var doctorId = slot.DoctorId;
        var icdCodeId = 1 + this._random.Next(0, ReferenceData.ICDCodes.Length);

        this._dbContext.Diagnoses.Add(new Diagnosis
        {
            Appointment = appointment,
            ICDCodeId = icdCodeId,
            DiagnosisDate = examinedAt,
            Notes = "Muayene bulguları doğrultusunda tanı konuldu."
        });

        var prescription = new Prescription
        {
            DoctorId = doctorId,
            Patient = patient,
            Appointment = appointment,
            PrescriptionDate = examinedAt,
            Notes = "Şikâyet devam ederse kontrole gelinmesi önerildi."
        };

        var medicinePool = new List<(string Name, string Usage)>
        {
            ("Parasetamol 500 mg", "Günde 3x1, tok karnına"),
            ("Amoksisilin 1000 mg", "Günde 2x1, 7 gün"),
            ("Pantoprazol 40 mg", "Günde 1x1, aç karnına"),
            ("Metformin 850 mg", "Günde 2x1, yemekle birlikte"),
            ("Ramipril 5 mg", "Günde 1x1, sabah"),
            ("Setirizin 10 mg", "Günde 1x1, akşam")
        };

        var medicineCount = 1 + this._random.Next(0, 3);
        var offset = this._random.Next(0, medicinePool.Count);

        for (var index = 0; index < medicineCount; index++)
        {
            var (name, usage) = medicinePool[(offset + index) % medicinePool.Count];
            prescription.Medicines.Add(new PrescriptionMedicine
            {
                Name = name,
                Description = "Demo reçete kalemi",
                Usage = usage
            });
        }

        this._dbContext.Prescriptions.Add(prescription);

        if (this._random.Next(0, 100) < 60)
        {
            var testPool = new List<(string Name, string Type)>
            {
                ("Tam Kan Sayımı", "Hematoloji"),
                ("Açlık Kan Şekeri", "Biyokimya"),
                ("EKG", "Kardiyoloji"),
                ("Akciğer Grafisi", "Radyoloji"),
                ("TSH", "Endokrinoloji")
            };

            var (testName, testType) = testPool[this._random.Next(0, testPool.Count)];
            var isCompleted = this._random.Next(0, 100) < 70;

            this._dbContext.LabTests.Add(new LabTest
            {
                Appointment = appointment,
                TestName = testName,
                TestType = testType,
                RequestedDate = examinedAt,
                ResultDate = isCompleted ? examinedAt.AddHours(6) : null,
                Result = isCompleted ? "Referans aralıklarında." : null,
                Status = isCompleted ? LabTestStatus.Completed : LabTestStatus.Requested,
                Notes = isCompleted ? null : "Sonuç bekleniyor."
            });
        }

        if (this._random.Next(0, 100) < 35)
        {
            var types = new List<MedicalHistoryType>
            {
                MedicalHistoryType.Chronic,
                MedicalHistoryType.Surgical,
                MedicalHistoryType.Allergic
            };

            var type = types[this._random.Next(0, types.Count)];

            this._dbContext.MedicalHistories.Add(new MedicalHistory
            {
                Patient = patient,
                Type = type,
                Description = type switch
                {
                    MedicalHistoryType.Chronic => "Kronik takip gerektiren durum saptandı.",
                    MedicalHistoryType.Surgical => "Geçirilmiş operasyon kaydı eklendi.",
                    _ => "İlaç alerjisi kaydedildi."
                },
                DiagnosedDate = examinedAt,
                RecordedByDoctorId = doctorId,
                Appointment = appointment
            });
        }
    }

    private static List<AppointmentSlot> PickEvenlySpaced(List<AppointmentSlot> source, int count)
    {
        // Spreading the picks keeps the demo appointments from piling onto one doctor.
        var step = Math.Max(1, source.Count / Math.Max(count, 1));

        return source
            .Where((_, index) => index % step == 0)
            .Take(count)
            .ToList();
    }
}
