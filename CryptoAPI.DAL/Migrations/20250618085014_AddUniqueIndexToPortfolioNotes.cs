using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoAPI.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToPortfolioNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CoinId",
                table: "PortfolioNotes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioNotes_UserId_CoinId",
                table: "PortfolioNotes",
                columns: new[] { "UserId", "CoinId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PortfolioNotes_UserId_CoinId",
                table: "PortfolioNotes");

            migrationBuilder.AlterColumn<string>(
                name: "CoinId",
                table: "PortfolioNotes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
