using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierCounter.Migrations
{
    /// <inheritdoc />
    public partial class addedstatusfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AllWorkers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "AllWorkers");
        }
    }
}
