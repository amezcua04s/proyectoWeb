using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hospitalAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contraseña = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fecha_nacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    sexo = table.Column<int>(type: "int", nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false),
                    admin = table.Column<bool>(type: "bit", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    especialidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    horario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    alergias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    enfermedades = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    medicamentos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    antecedentes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cirugias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tratamientos = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
