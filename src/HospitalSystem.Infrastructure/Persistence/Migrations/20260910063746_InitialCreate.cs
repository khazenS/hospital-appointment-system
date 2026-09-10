using System;
using HospitalSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HospitalSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:appointment_status", "Waiting,Cancelled,Being-diagnosed,Diagnosed")
                .Annotation("Npgsql:Enum:blood_type", "A+,A-,B+,B-,AB+,AB-,0+,0-")
                .Annotation("Npgsql:Enum:day_of_week", "Pazartesi,Salı,Çarşamba,Perşembe,Cuma,Cumartesi,Pazar")
                .Annotation("Npgsql:Enum:gender", "Male,Female")
                .Annotation("Npgsql:Enum:hospital_type", "Government,Primary,University")
                .Annotation("Npgsql:Enum:lab_test_status", "Requested,Waiting,Completed")
                .Annotation("Npgsql:Enum:medical_history_type", "chronic,surgical,allergic")
                .Annotation("Npgsql:Enum:slot_status", "Available,Booked,Blocked")
                .Annotation("Npgsql:Enum:title", "Dr.,Uzm. Dr.,Doç. Dr.,Prof. Dr.,Op. Dr.,Asist. Dr.");

            migrationBuilder.CreateTable(
                name: "Hospital",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Type = table.Column<HospitalType>(type: "hospital_type", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: true),
                    EstablishDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospital", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ICDCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ICDCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BloodType = table.Column<BloodType>(type: "blood_type", nullable: true),
                    Gender = table.Column<Gender>(type: "gender", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialty", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Policlinic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HospitalId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AppointmentTimePeriod = table.Column<TimeSpan>(type: "interval", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policlinic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Policlinic_Hospital_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospital",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmergencyContact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmergencyContact_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PoliclinicRoom",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PoliclinicId = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliclinicRoom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PoliclinicRoom_Policlinic_PoliclinicId",
                        column: x => x.PoliclinicId,
                        principalTable: "Policlinic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Doctor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PoliclinicRoomId = table.Column<int>(type: "integer", nullable: false),
                    HospitalId = table.Column<int>(type: "integer", nullable: false),
                    SpecialtyId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Age = table.Column<int>(type: "integer", nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Title = table.Column<DoctorTitle>(type: "title", nullable: false),
                    HireDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doctor_Hospital_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospital",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Doctor_PoliclinicRoom_PoliclinicRoomId",
                        column: x => x.PoliclinicRoomId,
                        principalTable: "PoliclinicRoom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Doctor_Specialty_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentSlot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DoctorId = table.Column<int>(type: "integer", nullable: false),
                    PoliclinicRoomId = table.Column<int>(type: "integer", nullable: false),
                    SlotDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Status = table.Column<SlotStatus>(type: "slot_status", nullable: false, defaultValue: SlotStatus.Available),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentSlot", x => x.Id);
                    table.CheckConstraint("CK_AppointmentSlot_TimeRange", "\"EndTime\" > \"StartTime\"");
                    table.ForeignKey(
                        name: "FK_AppointmentSlot_Doctor_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentSlot_PoliclinicRoom_PoliclinicRoomId",
                        column: x => x.PoliclinicRoomId,
                        principalTable: "PoliclinicRoom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoctorSchedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DoctorId = table.Column<int>(type: "integer", nullable: false),
                    DayName = table.Column<WeekDay>(type: "day_of_week", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ValidFrom = table.Column<DateOnly>(type: "date", nullable: true),
                    ValidTo = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorSchedule", x => x.Id);
                    table.CheckConstraint("CK_DoctorSchedule_TimeRange", "\"EndTime\" > \"StartTime\"");
                    table.CheckConstraint("CK_DoctorSchedule_ValidRange", "\"ValidTo\" IS NULL OR \"ValidFrom\" IS NULL OR \"ValidTo\" >= \"ValidFrom\"");
                    table.ForeignKey(
                        name: "FK_DoctorSchedule_Doctor_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    SlotId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<AppointmentStatus>(type: "appointment_status", nullable: false, defaultValue: AppointmentStatus.Waiting),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointment_AppointmentSlot_SlotId",
                        column: x => x.SlotId,
                        principalTable: "AppointmentSlot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointment_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Diagnosis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppointmentId = table.Column<int>(type: "integer", nullable: false),
                    ICDCodeId = table.Column<int>(type: "integer", nullable: false),
                    DiagnosisDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnosis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diagnosis_Appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Diagnosis_ICDCode_ICDCodeId",
                        column: x => x.ICDCodeId,
                        principalTable: "ICDCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LabTest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppointmentId = table.Column<int>(type: "integer", nullable: false),
                    TestName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    TestType = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    RequestedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResultDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Result = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<LabTestStatus>(type: "lab_test_status", nullable: false, defaultValue: LabTestStatus.Requested),
                    Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabTest_Appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<MedicalHistoryType>(type: "medical_history_type", nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    DiagnosedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RecordedByDoctorId = table.Column<int>(type: "integer", nullable: false),
                    AppointmentId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalHistory_Appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicalHistory_Doctor_RecordedByDoctorId",
                        column: x => x.RecordedByDoctorId,
                        principalTable: "Doctor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicalHistory_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prescription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DoctorId = table.Column<int>(type: "integer", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    AppointmentId = table.Column<int>(type: "integer", nullable: false),
                    PrescriptionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prescription_Appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prescription_Doctor_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescription_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionMedicine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PrescriptionId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Usage = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionMedicine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrescriptionMedicine_Prescription_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ICDCode",
                columns: new[] { "Id", "Code", "CreatedAt", "Description", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "I10", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Esansiyel (primer) hipertansiyon", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "I20.9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Angina pektoris, tanımlanmamış", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, "E11", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tip 2 diabetes mellitus", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, "E78.5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hiperlipidemi, tanımlanmamış", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, "J06.9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Akut üst solunum yolu enfeksiyonu, tanımlanmamış", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, "J45", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Astım", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, "K21.0", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Gastroözofageal reflü hastalığı, özofajitli", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, "A09", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Enfeksiyöz kaynaklı olduğu varsayılan gastroenterit ve kolit", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, "M54.5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bel ağrısı", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, "S52.5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Radius alt uç kırığı", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, "G43.9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Migren, tanımlanmamış", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, "R51", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Baş ağrısı", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 13, "N39.0", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Üriner sistem enfeksiyonu, yeri belirtilmemiş", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 14, "H52.1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Miyopi", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 15, "L20.9", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Atopik dermatit, tanımlanmamış", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Specialty",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kalp ve dolaşım sistemi hastalıkları", "Kardiyoloji", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "İç hastalıkları", "Dahiliye", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sinir sistemi hastalıkları", "Nöroloji", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kas-iskelet sistemi hastalıkları ve kırıklar", "Ortopedi ve Travmatoloji", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Göz ve görme bozuklukları", "Göz Hastalıkları", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "KBB hastalıkları", "Kulak Burun Boğaz", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pediatri", "Çocuk Sağlığı ve Hastalıkları", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cerrahi girişim gerektiren hastalıklar", "Genel Cerrahi", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Deri hastalıkları", "Dermatoloji", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Jinekoloji ve obstetri", "Kadın Hastalıkları ve Doğum", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_PatientId",
                table: "Appointment",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_SlotId",
                table: "Appointment",
                column: "SlotId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentSlot_PoliclinicRoomId",
                table: "AppointmentSlot",
                column: "PoliclinicRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentSlot_SlotDate_Status",
                table: "AppointmentSlot",
                columns: new[] { "SlotDate", "Status" });

            migrationBuilder.CreateIndex(
                name: "UX_AppointmentSlot_Doctor_Date_Start",
                table: "AppointmentSlot",
                columns: new[] { "DoctorId", "SlotDate", "StartTime" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Diagnosis_AppointmentId",
                table: "Diagnosis",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnosis_ICDCodeId",
                table: "Diagnosis",
                column: "ICDCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_HospitalId",
                table: "Doctor",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_PoliclinicRoomId",
                table: "Doctor",
                column: "PoliclinicRoomId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_SpecialtyId",
                table: "Doctor",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSchedule_DoctorId_DayName",
                table: "DoctorSchedule",
                columns: new[] { "DoctorId", "DayName" });

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyContact_PatientId",
                table: "EmergencyContact",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "UX_EmergencyContact_PhoneNumber",
                table: "EmergencyContact",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hospital_City",
                table: "Hospital",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "UX_Hospital_PhoneNumber",
                table: "Hospital",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_ICDCode_Code",
                table: "ICDCode",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LabTest_AppointmentId",
                table: "LabTest",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LabTest_Status",
                table: "LabTest",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistory_AppointmentId",
                table: "MedicalHistory",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistory_PatientId_DiagnosedDate",
                table: "MedicalHistory",
                columns: new[] { "PatientId", "DiagnosedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistory_RecordedByDoctorId",
                table: "MedicalHistory",
                column: "RecordedByDoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_Name",
                table: "Patient",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "UX_Patient_PhoneNumber",
                table: "Patient",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Policlinic_HospitalId",
                table: "Policlinic",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "UX_Policlinic_PhoneNumber",
                table: "Policlinic",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PoliclinicRoom_PoliclinicId",
                table: "PoliclinicRoom",
                column: "PoliclinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_AppointmentId",
                table: "Prescription",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_DoctorId",
                table: "Prescription",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_PatientId",
                table: "Prescription",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionMedicine_PrescriptionId",
                table: "PrescriptionMedicine",
                column: "PrescriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Diagnosis");

            migrationBuilder.DropTable(
                name: "DoctorSchedule");

            migrationBuilder.DropTable(
                name: "EmergencyContact");

            migrationBuilder.DropTable(
                name: "LabTest");

            migrationBuilder.DropTable(
                name: "MedicalHistory");

            migrationBuilder.DropTable(
                name: "PrescriptionMedicine");

            migrationBuilder.DropTable(
                name: "ICDCode");

            migrationBuilder.DropTable(
                name: "Prescription");

            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "AppointmentSlot");

            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropTable(
                name: "Doctor");

            migrationBuilder.DropTable(
                name: "PoliclinicRoom");

            migrationBuilder.DropTable(
                name: "Specialty");

            migrationBuilder.DropTable(
                name: "Policlinic");

            migrationBuilder.DropTable(
                name: "Hospital");
        }
    }
}
