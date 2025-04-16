using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISITECH__EventsArea.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Speakers_Id",
                table: "Speakers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_Id",
                table: "Sessions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_Id",
                table: "Rooms",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_Id",
                table: "Ratings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_Id",
                table: "Participants",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Id",
                table: "Locations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Events_Id",
                table: "Events",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_EventCategories_Id",
                table: "EventCategories",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Speakers_Id",
                table: "Speakers");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_Id",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_Id",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_Id",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Participants_Id",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Locations_Id",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Events_Id",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_EventCategories_Id",
                table: "EventCategories");
        }
    }
}
