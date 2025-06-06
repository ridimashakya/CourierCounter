using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierCounter.Migrations
{
    /// <inheritdoc />
    public partial class addedlatitudelongitudeandassignedHubZoneIdtosupportproximitybasedhubassignmentandorderfiltering : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DeliveryLatitude",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DeliveryLongitude",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "AssignedHubZoneId",
                table: "AllWorkers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "AllWorkers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "AllWorkers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryLatitude",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryLongitude",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AssignedHubZoneId",
                table: "AllWorkers");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "AllWorkers");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "AllWorkers");
        }
    }
}
