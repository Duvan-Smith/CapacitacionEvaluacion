using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Evaluacion.Infraestructura.Datos.Persistencia.Core.Migrations
{
    public partial class Guidentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_TipoDocumentos_TipoDocumentoEntityId",
                table: "Clientes");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Areas_AreaEntityId",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_TipoDocumentos_TipoDocumentoEntityId",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Proveedores_TipoDocumentos_TipoDocumentoEntityId",
                table: "Proveedores");

            migrationBuilder.DropIndex(
                name: "IX_Proveedores_TipoDocumentoEntityId",
                table: "Proveedores");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_AreaEntityId",
                table: "Empleados");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_TipoDocumentoEntityId",
                table: "Empleados");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_TipoDocumentoEntityId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "TipoDocumentoEntityId",
                table: "Proveedores");

            migrationBuilder.DropColumn(
                name: "AreaEntityId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "TipoDocumentoEntityId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "TipoDocumentoEntityId",
                table: "Clientes");

            migrationBuilder.AddColumn<Guid>(
                name: "TipoDocumentoId",
                table: "Proveedores",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AreaId",
                table: "Empleados",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TipoDocumentoId",
                table: "Empleados",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TipoDocumentoId",
                table: "Clientes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_TipoDocumentoId",
                table: "Proveedores",
                column: "TipoDocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_AreaId",
                table: "Empleados",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_TipoDocumentoId",
                table: "Empleados",
                column: "TipoDocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_TipoDocumentoId",
                table: "Clientes",
                column: "TipoDocumentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_TipoDocumentos_TipoDocumentoId",
                table: "Clientes",
                column: "TipoDocumentoId",
                principalTable: "TipoDocumentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Areas_AreaId",
                table: "Empleados",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_TipoDocumentos_TipoDocumentoId",
                table: "Empleados",
                column: "TipoDocumentoId",
                principalTable: "TipoDocumentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Proveedores_TipoDocumentos_TipoDocumentoId",
                table: "Proveedores",
                column: "TipoDocumentoId",
                principalTable: "TipoDocumentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_TipoDocumentos_TipoDocumentoId",
                table: "Clientes");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Areas_AreaId",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_TipoDocumentos_TipoDocumentoId",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Proveedores_TipoDocumentos_TipoDocumentoId",
                table: "Proveedores");

            migrationBuilder.DropIndex(
                name: "IX_Proveedores_TipoDocumentoId",
                table: "Proveedores");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_AreaId",
                table: "Empleados");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_TipoDocumentoId",
                table: "Empleados");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_TipoDocumentoId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "TipoDocumentoId",
                table: "Proveedores");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "TipoDocumentoId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "TipoDocumentoId",
                table: "Clientes");

            migrationBuilder.AddColumn<Guid>(
                name: "TipoDocumentoEntityId",
                table: "Proveedores",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AreaEntityId",
                table: "Empleados",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TipoDocumentoEntityId",
                table: "Empleados",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TipoDocumentoEntityId",
                table: "Clientes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_TipoDocumentoEntityId",
                table: "Proveedores",
                column: "TipoDocumentoEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_AreaEntityId",
                table: "Empleados",
                column: "AreaEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_TipoDocumentoEntityId",
                table: "Empleados",
                column: "TipoDocumentoEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_TipoDocumentoEntityId",
                table: "Clientes",
                column: "TipoDocumentoEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_TipoDocumentos_TipoDocumentoEntityId",
                table: "Clientes",
                column: "TipoDocumentoEntityId",
                principalTable: "TipoDocumentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Areas_AreaEntityId",
                table: "Empleados",
                column: "AreaEntityId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_TipoDocumentos_TipoDocumentoEntityId",
                table: "Empleados",
                column: "TipoDocumentoEntityId",
                principalTable: "TipoDocumentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Proveedores_TipoDocumentos_TipoDocumentoEntityId",
                table: "Proveedores",
                column: "TipoDocumentoEntityId",
                principalTable: "TipoDocumentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
