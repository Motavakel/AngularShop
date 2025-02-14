using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Portal");

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Order",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TransactionId",
                table: "Order",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Order");

            migrationBuilder.CreateTable(
                name: "Portal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gateway = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    ReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Portal_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Portal_OrderId",
                table: "Portal",
                column: "OrderId",
                unique: true);
        }
    }
}
