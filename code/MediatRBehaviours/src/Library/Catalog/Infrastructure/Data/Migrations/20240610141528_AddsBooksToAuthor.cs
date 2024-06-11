using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Catalog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddsBooksToAuthor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Books",
                schema: "Catalog",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Books",
                schema: "Catalog",
                table: "Authors");
        }
    }
}
