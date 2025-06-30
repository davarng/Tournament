using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournament.Data.Migrations
{
    /// <inheritdoc />
    public partial class referenceGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_TournamentDetails_TournamentDetailsId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_TournamentDetailsId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "TournamentDetailsId",
                table: "Games");

            migrationBuilder.CreateIndex(
                name: "IX_Games_TournamentId",
                table: "Games",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_TournamentDetails_TournamentId",
                table: "Games",
                column: "TournamentId",
                principalTable: "TournamentDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_TournamentDetails_TournamentId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_TournamentId",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "TournamentDetailsId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_TournamentDetailsId",
                table: "Games",
                column: "TournamentDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_TournamentDetails_TournamentDetailsId",
                table: "Games",
                column: "TournamentDetailsId",
                principalTable: "TournamentDetails",
                principalColumn: "Id");
        }
    }
}
