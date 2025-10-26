using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DIG103_Ticket_platform_back.Migrations
{
    /// <inheritdoc />
    public partial class AddImageSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "Background",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Artists");

            migrationBuilder.AddColumn<int>(
                name: "FeatureImageId",
                table: "Features",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventBackgroundId",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventImageId",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ArtistImageId",
                table: "Artists",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImagePath = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Features_FeatureImageId",
                table: "Features",
                column: "FeatureImageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventBackgroundId",
                table: "Events",
                column: "EventBackgroundId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventImageId",
                table: "Events",
                column: "EventImageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Artists_ArtistImageId",
                table: "Artists",
                column: "ArtistImageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Artists_Images_ArtistImageId",
                table: "Artists",
                column: "ArtistImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Images_EventBackgroundId",
                table: "Events",
                column: "EventBackgroundId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Images_EventImageId",
                table: "Events",
                column: "EventImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Images_FeatureImageId",
                table: "Features",
                column: "FeatureImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artists_Images_ArtistImageId",
                table: "Artists");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Images_EventBackgroundId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Images_EventImageId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Features_Images_FeatureImageId",
                table: "Features");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Features_FeatureImageId",
                table: "Features");

            migrationBuilder.DropIndex(
                name: "IX_Events_EventBackgroundId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_EventImageId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Artists_ArtistImageId",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "FeatureImageId",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "EventBackgroundId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventImageId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ArtistImageId",
                table: "Artists");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Features",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Background",
                table: "Events",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Events",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Artists",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
