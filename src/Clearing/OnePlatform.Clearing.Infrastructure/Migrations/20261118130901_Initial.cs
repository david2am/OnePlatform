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
            migrationBuilder.CreateTable(
                name: "Institutions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RoutingNumber = table.Column<string>(type: "nchar(9)", fixedLength: true, maxLength: 9, nullable: false),
                    IsMember = table.Column<bool>(type: "bit", nullable: false),
                    MembershipSince = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Institutions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageId = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    EndToEndId = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    ReturnOf = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    InitiatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    Currency = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false),
                    CreditorAccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditorInstitutionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreditorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DebtorAccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DebtorInstitutionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DebtorName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstitutionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountNumber = table.Column<string>(type: "varchar(34)", unicode: false, maxLength: 34, nullable: false),
                    Currency = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    OpenedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StatusEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusEvents_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ValidationResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheckType = table.Column<int>(type: "int", nullable: false),
                    Outcome = table.Column<int>(type: "int", nullable: false),
                    CheckedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidationResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValidationResults_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_InstitutionId_AccountNumber",
                table: "Accounts",
                columns: new[] { "InstitutionId", "AccountNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_InstitutionId_Currency",
                table: "Accounts",
                columns: new[] { "InstitutionId", "Currency" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Institutions_RoutingNumber",
                table: "Institutions",
                column: "RoutingNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatusEvents_PaymentId",
                table: "StatusEvents",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationResults_PaymentId",
                table: "ValidationResults",
                column: "PaymentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "StatusEvents");

            migrationBuilder.DropTable(
                name: "ValidationResults");

            migrationBuilder.DropTable(
                name: "Institutions");

            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
