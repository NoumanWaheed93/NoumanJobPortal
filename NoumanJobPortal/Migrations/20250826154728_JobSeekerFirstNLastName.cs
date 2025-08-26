using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoumanJobPortal.Migrations
{
    /// <inheritdoc />
    public partial class JobSeekerFirstNLastName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "Identity",
                table: "JobSeekers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "Identity",
                table: "JobSeekers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "Identity",
                table: "JobSeekers");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "Identity",
                table: "JobSeekers");
        }
    }
}
