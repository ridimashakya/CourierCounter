using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierCounter.Migrations
{
    /// <inheritdoc />
    public partial class addeddistanceweightandurgencylevelinorderstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "DistanceInKm",
                table: "Orders",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "UrgencyLevel",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Wage",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<float>(
                name: "WeightInKg",
                table: "Orders",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistanceInKm",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UrgencyLevel",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Wage",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "WeightInKg",
                table: "Orders");
        }
    }
}
