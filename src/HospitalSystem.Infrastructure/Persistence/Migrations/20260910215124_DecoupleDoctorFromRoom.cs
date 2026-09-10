using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DecoupleDoctorFromRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_PoliclinicRoom_PoliclinicRoomId",
                table: "Doctor");

            migrationBuilder.DropIndex(
                name: "IX_Doctor_PoliclinicRoomId",
                table: "Doctor");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentSlot_PoliclinicRoomId",
                table: "AppointmentSlot");

            // Not a rename: the old column held room ids, the new one holds policlinic ids.
            // Renaming would carry meaningless values over into the new foreign key.
            migrationBuilder.DropColumn(
                name: "PoliclinicRoomId",
                table: "Doctor");

            migrationBuilder.AddColumn<int>(
                name: "PoliclinicId",
                table: "Doctor",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PoliclinicRoomId",
                table: "DoctorSchedule",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSchedule_PoliclinicRoomId",
                table: "DoctorSchedule",
                column: "PoliclinicRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_PoliclinicId",
                table: "Doctor",
                column: "PoliclinicId");

            migrationBuilder.CreateIndex(
                name: "UX_AppointmentSlot_Room_Date_Start",
                table: "AppointmentSlot",
                columns: new[] { "PoliclinicRoomId", "SlotDate", "StartTime" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctor_Policlinic_PoliclinicId",
                table: "Doctor",
                column: "PoliclinicId",
                principalTable: "Policlinic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedule_PoliclinicRoom_PoliclinicRoomId",
                table: "DoctorSchedule",
                column: "PoliclinicRoomId",
                principalTable: "PoliclinicRoom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_Policlinic_PoliclinicId",
                table: "Doctor");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedule_PoliclinicRoom_PoliclinicRoomId",
                table: "DoctorSchedule");

            migrationBuilder.DropIndex(
                name: "IX_DoctorSchedule_PoliclinicRoomId",
                table: "DoctorSchedule");

            migrationBuilder.DropIndex(
                name: "IX_Doctor_PoliclinicId",
                table: "Doctor");

            migrationBuilder.DropIndex(
                name: "UX_AppointmentSlot_Room_Date_Start",
                table: "AppointmentSlot");

            migrationBuilder.DropColumn(
                name: "PoliclinicRoomId",
                table: "DoctorSchedule");

            migrationBuilder.DropColumn(
                name: "PoliclinicId",
                table: "Doctor");

            migrationBuilder.AddColumn<int>(
                name: "PoliclinicRoomId",
                table: "Doctor",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_PoliclinicRoomId",
                table: "Doctor",
                column: "PoliclinicRoomId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentSlot_PoliclinicRoomId",
                table: "AppointmentSlot",
                column: "PoliclinicRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctor_PoliclinicRoom_PoliclinicRoomId",
                table: "Doctor",
                column: "PoliclinicRoomId",
                principalTable: "PoliclinicRoom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
