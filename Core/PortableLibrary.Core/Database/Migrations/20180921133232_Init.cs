using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PortableLibrary.Core.Database.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookLibraries",
                columns: table => new
                {
                    Alias = table.Column<string>(maxLength: 50, nullable: false),
                    Position = table.Column<int>(nullable: true),
                    IsPublished = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    IsPublic = table.Column<bool>(nullable: false),
                    BooksLibraryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookLibraries", x => x.BooksLibraryId);
                });

            migrationBuilder.CreateTable(
                name: "ExternalBooks",
                columns: table => new
                {
                    Alias = table.Column<string>(maxLength: 50, nullable: false),
                    Position = table.Column<int>(nullable: true),
                    IsPublished = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    ExternalBookId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalBooks", x => x.ExternalBookId);
                });

            migrationBuilder.CreateTable(
                name: "TvShowsLibraries",
                columns: table => new
                {
                    Alias = table.Column<string>(maxLength: 50, nullable: false),
                    Position = table.Column<int>(nullable: true),
                    IsPublished = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    IsPublic = table.Column<bool>(nullable: false),
                    TvShowsLibraryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvShowsLibraries", x => x.TvShowsLibraryId);
                });

            migrationBuilder.CreateTable(
                name: "LibrariesBooks",
                columns: table => new
                {
                    Alias = table.Column<string>(maxLength: 50, nullable: false),
                    Position = table.Column<int>(nullable: true),
                    IsPublished = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsFavourite = table.Column<bool>(nullable: false),
                    IsProcessed = table.Column<bool>(nullable: false),
                    IsProcessing = table.Column<bool>(nullable: false),
                    IsProcessingPlanned = table.Column<bool>(nullable: false),
                    IsWaitingToBecomeGlobal = table.Column<bool>(nullable: false),
                    ReleaseDate = table.Column<DateTimeOffset>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    CoverImage = table.Column<string>(nullable: true),
                    LibraryBookId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BooksLibraryId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Author = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibrariesBooks", x => x.LibraryBookId);
                    table.ForeignKey(
                        name: "FK_LibrariesBooks_BookLibraries_BooksLibraryId",
                        column: x => x.BooksLibraryId,
                        principalTable: "BookLibraries",
                        principalColumn: "BooksLibraryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibrariesTvShows",
                columns: table => new
                {
                    Alias = table.Column<string>(maxLength: 50, nullable: false),
                    Position = table.Column<int>(nullable: true),
                    IsPublished = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsFavourite = table.Column<bool>(nullable: false),
                    IsProcessed = table.Column<bool>(nullable: false),
                    IsProcessing = table.Column<bool>(nullable: false),
                    IsProcessingPlanned = table.Column<bool>(nullable: false),
                    IsWaitingToBecomeGlobal = table.Column<bool>(nullable: false),
                    ReleaseDate = table.Column<DateTimeOffset>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    CoverImage = table.Column<string>(nullable: true),
                    LibraryTvShowId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TvShowsLibraryId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    IsClosed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibrariesTvShows", x => x.LibraryTvShowId);
                    table.ForeignKey(
                        name: "FK_LibrariesTvShows_TvShowsLibraries_TvShowsLibraryId",
                        column: x => x.TvShowsLibraryId,
                        principalTable: "TvShowsLibraries",
                        principalColumn: "TvShowsLibraryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibraryBookCategory",
                columns: table => new
                {
                    Alias = table.Column<string>(maxLength: 50, nullable: false),
                    Position = table.Column<int>(nullable: true),
                    IsPublished = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    LibraryBookCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentLibraryBookCategoryId = table.Column<int>(nullable: true),
                    LibraryBookId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsWaitingToBecomeGlobal = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryBookCategory", x => x.LibraryBookCategoryId);
                    table.ForeignKey(
                        name: "FK_LibraryBookCategory_LibrariesBooks_LibraryBookId",
                        column: x => x.LibraryBookId,
                        principalTable: "LibrariesBooks",
                        principalColumn: "LibraryBookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibraryBookCategory_LibraryBookCategory_ParentLibraryBookCategoryId",
                        column: x => x.ParentLibraryBookCategoryId,
                        principalTable: "LibraryBookCategory",
                        principalColumn: "LibraryBookCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LibraryBookGenre",
                columns: table => new
                {
                    Alias = table.Column<string>(maxLength: 50, nullable: false),
                    Position = table.Column<int>(nullable: true),
                    IsPublished = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    LibraryBookGenreId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LibraryBookId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsWaitingToBecomeGlobal = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryBookGenre", x => x.LibraryBookGenreId);
                    table.ForeignKey(
                        name: "FK_LibraryBookGenre_LibrariesBooks_LibraryBookId",
                        column: x => x.LibraryBookId,
                        principalTable: "LibrariesBooks",
                        principalColumn: "LibraryBookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibraryBooks2ExternalBooks",
                columns: table => new
                {
                    LibraryBook2ExternalBookId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LibraryBookId = table.Column<int>(nullable: false),
                    ExternalBookId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryBooks2ExternalBooks", x => x.LibraryBook2ExternalBookId);
                    table.ForeignKey(
                        name: "FK_LibraryBooks2ExternalBooks_ExternalBooks_ExternalBookId",
                        column: x => x.ExternalBookId,
                        principalTable: "ExternalBooks",
                        principalColumn: "ExternalBookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibraryBooks2ExternalBooks_LibrariesBooks_LibraryBookId",
                        column: x => x.LibraryBookId,
                        principalTable: "LibrariesBooks",
                        principalColumn: "LibraryBookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibraryTvShowCategory",
                columns: table => new
                {
                    Alias = table.Column<string>(maxLength: 50, nullable: false),
                    Position = table.Column<int>(nullable: true),
                    IsPublished = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    LibraryTvShowCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentLibraryTvShowCategoryId = table.Column<int>(nullable: true),
                    LibraryTvShowId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsWaitingToBecomeGlobal = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryTvShowCategory", x => x.LibraryTvShowCategoryId);
                    table.ForeignKey(
                        name: "FK_LibraryTvShowCategory_LibrariesTvShows_LibraryTvShowId",
                        column: x => x.LibraryTvShowId,
                        principalTable: "LibrariesTvShows",
                        principalColumn: "LibraryTvShowId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibraryTvShowCategory_LibraryTvShowCategory_ParentLibraryTvShowCategoryId",
                        column: x => x.ParentLibraryTvShowCategoryId,
                        principalTable: "LibraryTvShowCategory",
                        principalColumn: "LibraryTvShowCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LibraryTvShowGenre",
                columns: table => new
                {
                    Alias = table.Column<string>(maxLength: 50, nullable: false),
                    Position = table.Column<int>(nullable: true),
                    IsPublished = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    LibraryTvShowGenreId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LibraryTvShowId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsWaitingToBecomeGlobal = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryTvShowGenre", x => x.LibraryTvShowGenreId);
                    table.ForeignKey(
                        name: "FK_LibraryTvShowGenre_LibrariesTvShows_LibraryTvShowId",
                        column: x => x.LibraryTvShowId,
                        principalTable: "LibrariesTvShows",
                        principalColumn: "LibraryTvShowId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibraryTvShowSeason",
                columns: table => new
                {
                    Alias = table.Column<string>(maxLength: 50, nullable: false),
                    Position = table.Column<int>(nullable: true),
                    IsPublished = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    LibraryTvShowSeasonId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LibraryTvShowId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    CoverImage = table.Column<string>(nullable: true),
                    Index = table.Column<int>(nullable: false),
                    IsFavourite = table.Column<bool>(nullable: false),
                    IsWatchingPlanned = table.Column<bool>(nullable: false),
                    IsWaitingToBecomeGlobal = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryTvShowSeason", x => x.LibraryTvShowSeasonId);
                    table.ForeignKey(
                        name: "FK_LibraryTvShowSeason_LibrariesTvShows_LibraryTvShowId",
                        column: x => x.LibraryTvShowId,
                        principalTable: "LibrariesTvShows",
                        principalColumn: "LibraryTvShowId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibraryTvShowEpisode",
                columns: table => new
                {
                    Alias = table.Column<string>(maxLength: 50, nullable: false),
                    Position = table.Column<int>(nullable: true),
                    IsPublished = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    LibraryTvShowEpisodeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LibraryTvShowSeasonId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    CoverImage = table.Column<string>(nullable: true),
                    Index = table.Column<int>(nullable: false),
                    ReleaseDate = table.Column<DateTime>(nullable: false),
                    IsFavourite = table.Column<bool>(nullable: false),
                    IsWatched = table.Column<bool>(nullable: false),
                    IsWatchingPlanned = table.Column<bool>(nullable: false),
                    IsWaitingToBecomeGlobal = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryTvShowEpisode", x => x.LibraryTvShowEpisodeId);
                    table.ForeignKey(
                        name: "FK_LibraryTvShowEpisode_LibraryTvShowSeason_LibraryTvShowSeasonId",
                        column: x => x.LibraryTvShowSeasonId,
                        principalTable: "LibraryTvShowSeason",
                        principalColumn: "LibraryTvShowSeasonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LibrariesBooks_BooksLibraryId",
                table: "LibrariesBooks",
                column: "BooksLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_LibrariesTvShows_TvShowsLibraryId",
                table: "LibrariesTvShows",
                column: "TvShowsLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBookCategory_LibraryBookId",
                table: "LibraryBookCategory",
                column: "LibraryBookId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBookCategory_ParentLibraryBookCategoryId",
                table: "LibraryBookCategory",
                column: "ParentLibraryBookCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBookGenre_LibraryBookId",
                table: "LibraryBookGenre",
                column: "LibraryBookId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBooks2ExternalBooks_ExternalBookId",
                table: "LibraryBooks2ExternalBooks",
                column: "ExternalBookId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBooks2ExternalBooks_LibraryBookId",
                table: "LibraryBooks2ExternalBooks",
                column: "LibraryBookId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryTvShowCategory_LibraryTvShowId",
                table: "LibraryTvShowCategory",
                column: "LibraryTvShowId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryTvShowCategory_ParentLibraryTvShowCategoryId",
                table: "LibraryTvShowCategory",
                column: "ParentLibraryTvShowCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryTvShowEpisode_LibraryTvShowSeasonId",
                table: "LibraryTvShowEpisode",
                column: "LibraryTvShowSeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryTvShowGenre_LibraryTvShowId",
                table: "LibraryTvShowGenre",
                column: "LibraryTvShowId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryTvShowSeason_LibraryTvShowId",
                table: "LibraryTvShowSeason",
                column: "LibraryTvShowId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LibraryBookCategory");

            migrationBuilder.DropTable(
                name: "LibraryBookGenre");

            migrationBuilder.DropTable(
                name: "LibraryBooks2ExternalBooks");

            migrationBuilder.DropTable(
                name: "LibraryTvShowCategory");

            migrationBuilder.DropTable(
                name: "LibraryTvShowEpisode");

            migrationBuilder.DropTable(
                name: "LibraryTvShowGenre");

            migrationBuilder.DropTable(
                name: "ExternalBooks");

            migrationBuilder.DropTable(
                name: "LibrariesBooks");

            migrationBuilder.DropTable(
                name: "LibraryTvShowSeason");

            migrationBuilder.DropTable(
                name: "BookLibraries");

            migrationBuilder.DropTable(
                name: "LibrariesTvShows");

            migrationBuilder.DropTable(
                name: "TvShowsLibraries");
        }
    }
}
