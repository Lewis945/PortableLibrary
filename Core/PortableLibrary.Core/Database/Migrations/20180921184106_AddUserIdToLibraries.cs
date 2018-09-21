using Microsoft.EntityFrameworkCore.Migrations;

namespace PortableLibrary.Core.Database.Migrations
{
    public partial class AddUserIdToLibraries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "TvShowsLibraries",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "BookLibraries",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TvShowsLibraries_AppUserId",
                table: "TvShowsLibraries",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookLibraries_AppUserId",
                table: "BookLibraries",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookLibraries_AspNetUsers_AppUserId",
                table: "BookLibraries",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TvShowsLibraries_AspNetUsers_AppUserId",
                table: "TvShowsLibraries",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookLibraries_AspNetUsers_AppUserId",
                table: "BookLibraries");

            migrationBuilder.DropForeignKey(
                name: "FK_TvShowsLibraries_AspNetUsers_AppUserId",
                table: "TvShowsLibraries");

            migrationBuilder.DropIndex(
                name: "IX_TvShowsLibraries_AppUserId",
                table: "TvShowsLibraries");

            migrationBuilder.DropIndex(
                name: "IX_BookLibraries_AppUserId",
                table: "BookLibraries");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "TvShowsLibraries");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "BookLibraries");
        }
    }
}
