using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenUp.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePostandUserEntityName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "Datecreated",
                table: "Posts",
                newName: "DateCreated");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Posts",
                newName: "Datecreated");
        }
    }
}
