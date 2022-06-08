using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Health.Appointment.Domain.Storage.Sql.ReferenceData.Migrations
{
    public partial class addScheduleSaga : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nurses",
                columns: table => new
                {
                    NurseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nurses", x => x.NurseId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nurses");
        }
    }
}
