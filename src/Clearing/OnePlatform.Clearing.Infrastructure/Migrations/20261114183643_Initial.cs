using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnePlatform.Clearing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StatusEvent_Payments_PaymentId",
                table: "StatusEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_ValidationResult_Payments_PaymentId",
                table: "ValidationResult");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ValidationResult",
                table: "ValidationResult");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StatusEvent",
                table: "StatusEvent");

            migrationBuilder.RenameTable(
                name: "ValidationResult",
                newName: "ValidationResults");

            migrationBuilder.RenameTable(
                name: "StatusEvent",
                newName: "StatusEvents");

            migrationBuilder.RenameIndex(
                name: "IX_ValidationResult_PaymentId",
                table: "ValidationResults",
                newName: "IX_ValidationResults_PaymentId");

            migrationBuilder.RenameIndex(
                name: "IX_StatusEvent_PaymentId",
                table: "StatusEvents",
                newName: "IX_StatusEvents_PaymentId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReturnOf",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ValidationResults",
                table: "ValidationResults",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StatusEvents",
                table: "StatusEvents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StatusEvents_Payments_PaymentId",
                table: "StatusEvents",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ValidationResults_Payments_PaymentId",
                table: "ValidationResults",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StatusEvents_Payments_PaymentId",
                table: "StatusEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_ValidationResults_Payments_PaymentId",
                table: "ValidationResults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ValidationResults",
                table: "ValidationResults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StatusEvents",
                table: "StatusEvents");

            migrationBuilder.RenameTable(
                name: "ValidationResults",
                newName: "ValidationResult");

            migrationBuilder.RenameTable(
                name: "StatusEvents",
                newName: "StatusEvent");

            migrationBuilder.RenameIndex(
                name: "IX_ValidationResults_PaymentId",
                table: "ValidationResult",
                newName: "IX_ValidationResult_PaymentId");

            migrationBuilder.RenameIndex(
                name: "IX_StatusEvents_PaymentId",
                table: "StatusEvent",
                newName: "IX_StatusEvent_PaymentId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReturnOf",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ValidationResult",
                table: "ValidationResult",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StatusEvent",
                table: "StatusEvent",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StatusEvent_Payments_PaymentId",
                table: "StatusEvent",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ValidationResult_Payments_PaymentId",
                table: "ValidationResult",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
