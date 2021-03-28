using Microsoft.EntityFrameworkCore.Migrations;

namespace Evaluacion.Infraestructura.Datos.Persistencia.Core.Migrations
{
    public partial class CodigoEmpleado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CodigoEmpleado",
                table: "Empleados",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoEmpleado",
                table: "Empleados");
        }
    }
}
