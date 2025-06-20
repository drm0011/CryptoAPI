using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoAPI.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddMoodToPortfolioNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "Mood",
                table: "PortfolioNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mood",
                table: "PortfolioNotes");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "PortfolioItem",
                type: "decimal(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
