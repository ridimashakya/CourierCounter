using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierCounter.Migrations
{
    /// <inheritdoc />
    public partial class addedimagefieldstoWorkers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LicenseNumberImagePath",
                table: "AllWorkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NationalIdNumberImagePath",
                table: "AllWorkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VehicleRegistrationNumberImagePath",
                table: "AllWorkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicenseNumberImagePath",
                table: "AllWorkers");

            migrationBuilder.DropColumn(
                name: "NationalIdNumberImagePath",
                table: "AllWorkers");

            migrationBuilder.DropColumn(
                name: "VehicleRegistrationNumberImagePath",
                table: "AllWorkers");
        }
    }
}
