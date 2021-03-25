using Microsoft.EntityFrameworkCore.Migrations;

namespace Evaluacion.Infraestructura.Datos.Persistencia.Core.Migrations
{
    public partial class CorrecionTipoDocumento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoTipoDocumento",
                table: "TipoDocumentos");

            migrationBuilder.AddColumn<string>(
                name: "CodigoTipoDocumento",
                table: "Proveedores",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodigoTipoDocumento",
                table: "Empleados",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodigoTipoDocumento",
                table: "Clientes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoTipoDocumento",
                table: "Proveedores");

            migrationBuilder.DropColumn(
                name: "CodigoTipoDocumento",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "CodigoTipoDocumento",
                table: "Clientes");

            migrationBuilder.AddColumn<string>(
                name: "CodigoTipoDocumento",
                table: "TipoDocumentos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
