using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Evaluacion.Infraestructura.Datos.Persistencia.Core.Migrations
{
    public partial class DeleteidFail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "TipoDocumentos");

            migrationBuilder.DropColumn(
                name: "EmpleadoId",
                table: "TipoDocumentos");

            migrationBuilder.DropColumn(
                name: "ProveedorId",
                table: "TipoDocumentos");

            migrationBuilder.DropColumn(
                name: "EmpleadoId",
                table: "Areas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClienteId",
                table: "TipoDocumentos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EmpleadoId",
                table: "TipoDocumentos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProveedorId",
                table: "TipoDocumentos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EmpleadoId",
                table: "Areas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
