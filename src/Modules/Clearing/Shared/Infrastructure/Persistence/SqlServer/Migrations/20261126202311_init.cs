using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnePlatform.Clearing.Shared.Infrastructure.Persistence.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
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
                    RoutingNumber = table.Column<string>(type: "char(9)", unicode: false, nullable: false),
                    IsMember = table.Column<bool>(type: "bit", nullable: false),
                    MembershipSince = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Institutions", x => x.Id);
                    table.CheckConstraint("CK_Institutions_Membership", "[IsMember] = 0 OR [MembershipSince] IS NOT NULL");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageId = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    EndToEndId = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    ReturnOf = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    Currency = table.Column<string>(type: "char(3)", nullable: false),
                    SenderInstitutionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DebtorAccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DebtorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DebtorInstitutionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreditorAccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreditorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreditorInstitutionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    InitiatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Payments_ReturnOf",
                        column: x => x.ReturnOf,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstitutionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountNumber = table.Column<string>(type: "varchar(17)", unicode: false, maxLength: 17, nullable: false),
                    Currency = table.Column<string>(type: "char(3)", nullable: false),
                    OpenedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.UniqueConstraint("AK_Accounts_Id_InstitutionId", x => new { x.Id, x.InstitutionId });
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
                    EventType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
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
                    CheckType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Outcome = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
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

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstitutionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                    table.UniqueConstraint("AK_Participants_Id_InstitutionId", x => new { x.Id, x.InstitutionId });
                    table.ForeignKey(
                        name: "FK_Participants_Accounts_AccountId_InstitutionId",
                        columns: x => new { x.AccountId, x.InstitutionId },
                        principalTable: "Accounts",
                        principalColumns: new[] { "Id", "InstitutionId" },
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_Participants_AccountId_InstitutionId",
                table: "Participants",
                columns: new[] { "AccountId", "InstitutionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Participants_InstitutionId",
                table: "Participants",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_PaymentId_InstitutionId",
                table: "Participants",
                columns: new[] { "PaymentId", "InstitutionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_EndToEndId",
                table: "Payments",
                column: "EndToEndId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ReturnOf",
                table: "Payments",
                column: "ReturnOf");

            migrationBuilder.CreateIndex(
                name: "UX_Payments_Sender_Message_EndToEnd",
                table: "Payments",
                columns: new[] { "SenderInstitutionId", "MessageId", "EndToEndId" },
                unique: true,
                filter: "[EndToEndId] <> 'NOTPROVIDED'");

            migrationBuilder.CreateIndex(
                name: "IX_StatusEvents_PaymentId_OccurredAt",
                table: "StatusEvents",
                columns: new[] { "PaymentId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ValidationResults_PaymentId_CheckedAt",
                table: "ValidationResults",
                columns: new[] { "PaymentId", "CheckedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "StatusEvents");

            migrationBuilder.DropTable(
                name: "ValidationResults");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Institutions");
        }
    }
}
