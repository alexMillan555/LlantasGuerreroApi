using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LlantasGuerreroApi.Migrations
{
    /// <inheritdoc />
    public partial class ModificacionBD210620250111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdEstatus",
                table: "CatArticulos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdEstatus",
                table: "CatArticulos");
        }
    }
}
