using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WorkoutTracker.Migrations
{
    /// <inheritdoc />
    public partial class NewModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // The schema was remodeled from scratch, so the old tables are dropped instead of altered
            migrationBuilder.DropTable(
                name: "Workouts");

            migrationBuilder.DropTable(
                name: "athletes");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    gender = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

            // Foreign keys to users and sports are added at the end, once sports exists
            migrationBuilder.CreateTable(
                name: "workouts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sport_id = table.Column<int>(type: "integer", nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    duration_seconds = table.Column<int>(type: "integer", nullable: true),
                    distance_km = table.Column<double>(type: "double precision", nullable: true),
                    avg_heart_rate = table.Column<int>(type: "integer", nullable: true),
                    calories = table.Column<int>(type: "integer", nullable: true),
                    perceived_effort = table.Column<int>(type: "integer", nullable: true),
                    notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_workouts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "body_measurements",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    height_cm = table.Column<double>(type: "double precision", nullable: true),
                    weight_kg = table.Column<double>(type: "double precision", nullable: true),
                    measured_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_body_measurements", x => x.id);
                    table.ForeignKey(
                        name: "fk_body_measurements_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sports",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_composite = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sports", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_profiles",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bio = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    profile_image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_profiles", x => x.user_id);
                    table.ForeignKey(
                        name: "fk_user_profiles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sport_segments",
                columns: table => new
                {
                    composite_sport_id = table.Column<int>(type: "integer", nullable: false),
                    segment_order = table.Column<int>(type: "integer", nullable: false),
                    discipline_id = table.Column<int>(type: "integer", nullable: false),
                    distance_km = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sport_segments", x => new { x.composite_sport_id, x.segment_order });
                    table.ForeignKey(
                        name: "fk_sport_segments_sports_composite_sport_id",
                        column: x => x.composite_sport_id,
                        principalTable: "sports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_sport_segments_sports_discipline_id",
                        column: x => x.discipline_id,
                        principalTable: "sports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "workout_segments",
                columns: table => new
                {
                    workout_id = table.Column<Guid>(type: "uuid", nullable: false),
                    segment_order = table.Column<int>(type: "integer", nullable: false),
                    discipline_id = table.Column<int>(type: "integer", nullable: false),
                    duration_seconds = table.Column<int>(type: "integer", nullable: true),
                    distance_km = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_workout_segments", x => new { x.workout_id, x.segment_order });
                    table.ForeignKey(
                        name: "fk_workout_segments_sports_discipline_id",
                        column: x => x.discipline_id,
                        principalTable: "sports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_workout_segments_workouts_workout_id",
                        column: x => x.workout_id,
                        principalTable: "workouts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" }
                });

            migrationBuilder.InsertData(
                table: "sports",
                columns: new[] { "id", "is_composite", "name" },
                values: new object[,]
                {
                    { 1, false, "Running" },
                    { 2, false, "Cycling" },
                    { 3, false, "Swimming" },
                    { 4, false, "Walking" },
                    { 5, false, "Hiking" },
                    { 6, false, "Rowing" },
                    { 7, false, "Strength" },
                    { 8, false, "Other" },
                    { 9, true, "Triathlon" },
                    { 10, true, "Duathlon" }
                });

            migrationBuilder.InsertData(
                table: "sport_segments",
                columns: new[] { "composite_sport_id", "segment_order", "discipline_id", "distance_km" },
                values: new object[,]
                {
                    { 9, 1, 3, 1.5 },
                    { 9, 2, 2, 40.0 },
                    { 9, 3, 1, 10.0 },
                    { 10, 1, 1, 10.0 },
                    { 10, 2, 2, 40.0 },
                    { 10, 3, 1, 5.0 }
                });

            migrationBuilder.CreateIndex(
                name: "ix_workouts_sport_id",
                table: "workouts",
                column: "sport_id");

            migrationBuilder.CreateIndex(
                name: "ix_workouts_user_id",
                table: "workouts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_body_measurements_user_id",
                table: "body_measurements",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_roles_name",
                table: "roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sport_segments_discipline_id",
                table: "sport_segments",
                column: "discipline_id");

            migrationBuilder.CreateIndex(
                name: "ix_sports_name",
                table: "sports",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_role_id",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_workout_segments_discipline_id",
                table: "workout_segments",
                column: "discipline_id");

            migrationBuilder.AddForeignKey(
                name: "fk_workouts_sports_sport_id",
                table: "workouts",
                column: "sport_id",
                principalTable: "sports",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_workouts_users_user_id",
                table: "workouts",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_workouts_sports_sport_id",
                table: "workouts");

            migrationBuilder.DropForeignKey(
                name: "fk_workouts_users_user_id",
                table: "workouts");

            migrationBuilder.DropTable(
                name: "body_measurements");

            migrationBuilder.DropTable(
                name: "sport_segments");

            migrationBuilder.DropTable(
                name: "user_profiles");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "workout_segments");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "sports");

            migrationBuilder.DropPrimaryKey(
                name: "pk_workouts",
                table: "workouts");

            migrationBuilder.DropIndex(
                name: "ix_workouts_sport_id",
                table: "workouts");

            migrationBuilder.DropIndex(
                name: "ix_workouts_user_id",
                table: "workouts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                table: "users");

            migrationBuilder.DropColumn(
                name: "calories",
                table: "workouts");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "workouts");

            migrationBuilder.DropColumn(
                name: "duration_seconds",
                table: "workouts");

            migrationBuilder.DropColumn(
                name: "notes",
                table: "workouts");

            migrationBuilder.DropColumn(
                name: "sport_id",
                table: "workouts");

            migrationBuilder.DropColumn(
                name: "started_at",
                table: "workouts");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "workouts");

            migrationBuilder.DropColumn(
                name: "date_of_birth",
                table: "users");

            migrationBuilder.DropColumn(
                name: "first_name",
                table: "users");

            migrationBuilder.DropColumn(
                name: "gender",
                table: "users");

            migrationBuilder.RenameTable(
                name: "workouts",
                newName: "Workouts");

            migrationBuilder.RenameColumn(
                name: "avg_heart_rate",
                table: "Workouts",
                newName: "avg_heartrate");

            migrationBuilder.RenameColumn(
                name: "perceived_effort",
                table: "Workouts",
                newName: "duration");

            migrationBuilder.RenameColumn(
                name: "distance_km",
                table: "Workouts",
                newName: "distance");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "users",
                newName: "username");

            migrationBuilder.RenameIndex(
                name: "ix_users_email",
                table: "users",
                newName: "IX_users_email");

            migrationBuilder.AddColumn<Guid>(
                name: "athlete_id",
                table: "Workouts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sport",
                table: "Workouts",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_login_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "User");

            migrationBuilder.AddPrimaryKey(
                name: "Workouts_pkey",
                table: "Workouts",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "users_pkey",
                table: "users",
                column: "id");

            migrationBuilder.CreateTable(
                name: "athletes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    age = table.Column<int>(type: "integer", nullable: true),
                    country = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    height = table.Column<double>(type: "double precision", nullable: true),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    surname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    weight = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("athletes_pkey", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_athlete_id",
                table: "Workouts",
                column: "athlete_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_athletes_athlete_id",
                table: "Workouts",
                column: "athlete_id",
                principalTable: "athletes",
                principalColumn: "id");
        }
    }
}
