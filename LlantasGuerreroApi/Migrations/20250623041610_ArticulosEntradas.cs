using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LlantasGuerreroApi.Migrations
{
    /// <inheritdoc />
    public partial class ArticulosEntradas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticulosEntradas",
                columns: table => new
                {
                    IdArticuloEntrada = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdArticulo = table.Column<int>(type: "int", nullable: false),
                    ArticuloEntradaImporte = table.Column<float>(type: "real", nullable: false),
                    ArticuloEntradaCantidad = table.Column<int>(type: "int", nullable: false),
                    IdMovimiento = table.Column<int>(type: "int", nullable: false),
                    FechaEntradaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArticuloEntradaObservaciones = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticulosEntradas", x => x.IdArticuloEntrada);
                    table.ForeignKey(
                        name: "FK_ArticulosEntradas_CatArticulos_IdArticulo",
                        column: x => x.IdArticulo,
                        principalTable: "CatArticulos",
                        principalColumn: "IdArticulo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticulosEntradas_Movimientos_IdMovimiento",
                        column: x => x.IdMovimiento,
                        principalTable: "Movimientos",
                        principalColumn: "IdMovimiento",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticulosEntradas_IdArticulo",
                table: "ArticulosEntradas",
                column: "IdArticulo");

            migrationBuilder.CreateIndex(
                name: "IX_ArticulosEntradas_IdMovimiento",
                table: "ArticulosEntradas",
                column: "IdMovimiento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticulosEntradas");
        }
    }
}
