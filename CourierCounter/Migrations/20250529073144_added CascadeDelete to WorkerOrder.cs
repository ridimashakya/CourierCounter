using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourierCounter.Migrations
{
    /// <inheritdoc />
    public partial class addedCascadeDeletetoWorkerOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WorkerOrder_OrderId",
                table: "WorkerOrder",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerOrder_WorkerId",
                table: "WorkerOrder",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerOrder_AllWorkers_WorkerId",
                table: "WorkerOrder",
                column: "WorkerId",
                principalTable: "AllWorkers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerOrder_Orders_OrderId",
                table: "WorkerOrder",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerOrder_AllWorkers_WorkerId",
                table: "WorkerOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerOrder_Orders_OrderId",
                table: "WorkerOrder");

            migrationBuilder.DropIndex(
                name: "IX_WorkerOrder_OrderId",
                table: "WorkerOrder");

            migrationBuilder.DropIndex(
                name: "IX_WorkerOrder_WorkerId",
                table: "WorkerOrder");
        }
    }
}
