using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenUp.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Notifivation_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Notifications",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Notifications");
        }
    }
}
