using Microsoft.EntityFrameworkCore.Migrations;

namespace control.personal.Data.Migrations
{
    public partial class identificacionendbset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Identificacion",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    idUsuario = table.Column<int>(type: "INTEGER", nullable: false),
                    Uid = table.Column<string>(type: "TEXT", nullable: true),
                    Estado = table.Column<int>(type: "INTEGER", nullable: false),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identificacion", x => x.id);
                    table.ForeignKey(
                        name: "FK_Identificacion_AspNetUsers_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Identificacion_idUsuario",
                table: "Identificacion",
                column: "idUsuario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Identificacion");
        }
    }
}
