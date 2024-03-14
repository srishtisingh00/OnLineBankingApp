using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnLineBankingApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedStatusUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Beneficiaries",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.InsertData(
                table: "Statuss",
                columns: new[] { "StatusID", "StatusType" },
                values: new object[,]
                {
                    { 6, "Success" },
                    { 7, "Failed" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Statuss",
                keyColumn: "StatusID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Statuss",
                keyColumn: "StatusID",
                keyValue: 7);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Beneficiaries",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
