using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBudgetManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNullableintonotefield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "DebtAndLoanId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "DebtAndLoans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "DebtAndLoans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "DebtAndLoans",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "DebtAndLoans",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_DebtAndLoanId",
                table: "Transactions",
                column: "DebtAndLoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_DebtAndLoans_DebtAndLoanId",
                table: "Transactions",
                column: "DebtAndLoanId",
                principalTable: "DebtAndLoans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_DebtAndLoans_DebtAndLoanId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_DebtAndLoanId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "DebtAndLoanId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "DebtAndLoans");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DebtAndLoans");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "DebtAndLoans");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "DebtAndLoans");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
