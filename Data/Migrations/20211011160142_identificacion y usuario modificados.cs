using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace control.personal.Data.Migrations
{
    public partial class identificacionyusuariomodificados : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "AspNetUserTokens",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "Cedula",
                table: "AspNetUsers",
                type: "TEXT",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "AspNetUserRoles",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "AspNetUserRoles",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "AspNetUserLogins",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "AspNetUserClaims",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AspNetRoles",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "AspNetRoleClaims",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "Registro",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FechaHora = table.Column<DateTime>(type: "TEXT", nullable: false),
                    idUsuario = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registro", x => x.id);
                    table.ForeignKey(
                        name: "FK_Registro_AspNetUsers_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ControlIngreso",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UniformeLimpio = table.Column<bool>(type: "INTEGER", nullable: false),
                    UniformeCompleto = table.Column<bool>(type: "INTEGER", nullable: false),
                    UniformeColocadoBien = table.Column<bool>(type: "INTEGER", nullable: false),
                    ManoLimpias = table.Column<bool>(type: "INTEGER", nullable: false),
                    ManoSinAccesorios = table.Column<bool>(type: "INTEGER", nullable: false),
                    Observaciones = table.Column<string>(type: "TEXT", nullable: true),
                    Responsable = table.Column<string>(type: "TEXT", nullable: true),
                    IdRegistro = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlIngreso", x => x.id);
                    table.ForeignKey(
                        name: "FK_ControlIngreso_Registro_IdRegistro",
                        column: x => x.IdRegistro,
                        principalTable: "Registro",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Cedula",
                table: "AspNetUsers",
                column: "Cedula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ControlIngreso_IdRegistro",
                table: "ControlIngreso",
                column: "IdRegistro",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Registro_idUsuario",
                table: "Registro",
                column: "idUsuario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ControlIngreso");

            migrationBuilder.DropTable(
                name: "Registro");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Cedula",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Cedula",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserTokens",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AspNetUserRoles",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserRoles",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserLogins",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserClaims",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetRoles",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AspNetRoleClaims",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
