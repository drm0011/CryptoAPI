using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoAPI.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToPortfolioItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PortfolioItem_PortfolioId",
                table: "PortfolioItem");

            migrationBuilder.AlterColumn<string>(
                name: "CoinId",
                table: "PortfolioItem",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioItem_PortfolioId_CoinId",
                table: "PortfolioItem",
                columns: new[] { "PortfolioId", "CoinId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PortfolioItem_PortfolioId_CoinId",
                table: "PortfolioItem");

            migrationBuilder.AlterColumn<string>(
                name: "CoinId",
                table: "PortfolioItem",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioItem_PortfolioId",
                table: "PortfolioItem",
                column: "PortfolioId");
        }
    }
}
