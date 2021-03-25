using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Evaluacion.Infraestructura.Datos.Persistencia.Core.Migrations
{
    public partial class AddClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClienteEntityId",
                table: "Personas",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personas_ClienteEntityId",
                table: "Personas",
                column: "ClienteEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Personas_Clientes_ClienteEntityId",
                table: "Personas",
                column: "ClienteEntityId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personas_Clientes_ClienteEntityId",
                table: "Personas");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Personas_ClienteEntityId",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "ClienteEntityId",
                table: "Personas");
        }
    }
}
