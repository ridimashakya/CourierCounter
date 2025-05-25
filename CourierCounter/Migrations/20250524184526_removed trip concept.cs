using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierCounter.Migrations
{
    /// <inheritdoc />
    public partial class removedtripconcept : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Trip_TripId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Trip");

            migrationBuilder.DropTable(
                name: "TripOrder");

            migrationBuilder.DropIndex(
                name: "IX_Orders_TripId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SegmentDistance",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "SegmentDistance",
                table: "Orders",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "TripId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Trip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalDistance = table.Column<float>(type: "real", nullable: false),
                    TotalWages = table.Column<float>(type: "real", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trip", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TripOrder",
                columns: table => new
                {
                    TripId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    SegmentDistance = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripOrder", x => new { x.TripId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_TripOrder_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TripId",
                table: "Orders",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_TripOrder_OrderId",
                table: "TripOrder",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Trip_TripId",
                table: "Orders",
                column: "TripId",
                principalTable: "Trip",
                principalColumn: "Id");
        }
    }
}
