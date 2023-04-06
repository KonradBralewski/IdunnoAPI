using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdunnoAPI.Migrations
{
    /// <inheritdoc />
    public partial class isGlobalMessageProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isGlobalMessage",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isGlobalMessage",
                table: "Messages");
        }
    }
}
