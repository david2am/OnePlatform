using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnePlatform.Clearing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ParticipantsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ValidationResults_PaymentId",
                table: "ValidationResults");

            migrationBuilder.DropIndex(
                name: "IX_StatusEvents_PaymentId",
                table: "StatusEvents");

            migrationBuilder.AlterColumn<string>(
                name: "Outcome",
                table: "ValidationResults",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "CheckType",
                table: "ValidationResults",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "EventType",
                table: "StatusEvents",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "StatusEvents",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentType",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DebtorName",
                table: "Payments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DebtorAccountNumber",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Payments",
                type: "char(3)",
                unicode: false,
                fixedLength: true,
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(3)",
                oldUnicode: false,
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "CreditorName",
                table: "Payments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreditorAccountNumber",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "RoutingNumber",
                table: "Institutions",
                type: "char(9)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(9)",
                oldFixedLength: true,
                oldMaxLength: 9);

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstitutionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participants_Institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Participants_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ValidationResults_PaymentId_CheckedAt",
                table: "ValidationResults",
                columns: new[] { "PaymentId", "CheckedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_StatusEvents_PaymentId_OccurredAt",
                table: "StatusEvents",
                columns: new[] { "PaymentId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ReturnOf",
                table: "Payments",
                column: "ReturnOf");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Institutions_Membership",
                table: "Institutions",
                sql: "[IsMember] = 0 OR [MembershipSince] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_InstitutionId",
                table: "Participants",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_PaymentId_InstitutionId",
                table: "Participants",
                columns: new[] { "PaymentId", "InstitutionId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Payments_ReturnOf",
                table: "Payments",
                column: "ReturnOf",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Payments_ReturnOf",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_ValidationResults_PaymentId_CheckedAt",
                table: "ValidationResults");

            migrationBuilder.DropIndex(
                name: "IX_StatusEvents_PaymentId_OccurredAt",
                table: "StatusEvents");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ReturnOf",
                table: "Payments");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Institutions_Membership",
                table: "Institutions");

            migrationBuilder.AlterColumn<int>(
                name: "Outcome",
                table: "ValidationResults",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<int>(
                name: "CheckType",
                table: "ValidationResults",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "EventType",
                table: "StatusEvents",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "StatusEvents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Payments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentType",
                table: "Payments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DebtorName",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "DebtorAccountNumber",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Payments",
                type: "varchar(3)",
                unicode: false,
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(3)",
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "CreditorName",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "CreditorAccountNumber",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "RoutingNumber",
                table: "Institutions",
                type: "nchar(9)",
                fixedLength: true,
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(9)",
                oldUnicode: false);

            migrationBuilder.CreateIndex(
                name: "IX_ValidationResults_PaymentId",
                table: "ValidationResults",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusEvents_PaymentId",
                table: "StatusEvents",
                column: "PaymentId");
        }
    }
}
