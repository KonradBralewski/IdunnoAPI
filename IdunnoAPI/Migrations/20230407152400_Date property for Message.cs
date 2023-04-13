using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdunnoAPI.Migrations
{
    /// <inheritdoc />
    public partial class DatepropertyforMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MsgDate",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MsgDate",
                table: "Messages");
        }
    }
}
