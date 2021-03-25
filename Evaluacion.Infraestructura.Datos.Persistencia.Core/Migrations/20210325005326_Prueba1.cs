using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Evaluacion.Infraestructura.Datos.Persistencia.Core.Migrations
{
    public partial class Prueba1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoDocumentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NombreTipoDocumento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CodigoTipoDocumento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDocumentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AreaEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaNacimiento = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FechaRegistro = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    NumeroTelefono = table.Column<int>(type: "int", nullable: false),
                    CorreoElectronico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpleadoEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProveedorEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TipoDocumentoEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Personas_Empleados_EmpleadoEntityId",
                        column: x => x.EmpleadoEntityId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Personas_Proveedores_ProveedorEntityId",
                        column: x => x.ProveedorEntityId,
                        principalTable: "Proveedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Personas_TipoDocumentos_TipoDocumentoEntityId",
                        column: x => x.TipoDocumentoEntityId,
                        principalTable: "TipoDocumentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NombreArea = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PersonaEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Areas_Personas_PersonaEntityId",
                        column: x => x.PersonaEntityId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Areas_PersonaEntityId",
                table: "Areas",
                column: "PersonaEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_AreaEntityId",
                table: "Empleados",
                column: "AreaEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Personas_EmpleadoEntityId",
                table: "Personas",
                column: "EmpleadoEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Personas_ProveedorEntityId",
                table: "Personas",
                column: "ProveedorEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Personas_TipoDocumentoEntityId",
                table: "Personas",
                column: "TipoDocumentoEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Areas_AreaEntityId",
                table: "Empleados",
                column: "AreaEntityId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Personas_PersonaEntityId",
                table: "Areas");

            migrationBuilder.DropTable(
                name: "Personas");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropTable(
                name: "TipoDocumentos");

            migrationBuilder.DropTable(
                name: "Areas");
        }
    }
}
