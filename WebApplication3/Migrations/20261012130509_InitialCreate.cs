using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication3.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "athletes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    surname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    country = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    age = table.Column<int>(type: "integer", nullable: true),
                    height = table.Column<double>(type: "double precision", nullable: true),
                    weight = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("athletes_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Workouts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    sport = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    distance = table.Column<double>(type: "double precision", nullable: true),
                    duration = table.Column<int>(type: "integer", nullable: true),
                    avg_heartrate = table.Column<int>(type: "integer", nullable: true),
                    athlete_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Workouts_pkey", x => x.id);
                    table.ForeignKey(
                        name: "FK_Workouts_athletes_athlete_id",
                        column: x => x.athlete_id,
                        principalTable: "athletes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_athlete_id",
                table: "Workouts",
                column: "athlete_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workouts");

            migrationBuilder.DropTable(
                name: "athletes");
        }
    }
}
