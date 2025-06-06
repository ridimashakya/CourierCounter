using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierCounter.Migrations
{
    /// <inheritdoc />
    public partial class addedWageTrainingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WageTrainingDataset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Zone = table.Column<int>(type: "int", nullable: false),
                    DistanceInKm = table.Column<float>(type: "real", nullable: false),
                    WeightInKg = table.Column<float>(type: "real", nullable: false),
                    UrgencyLevel = table.Column<float>(type: "real", nullable: false),
                    Wage = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WageTrainingDataset", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WageTrainingDataset");
        }
    }
}
