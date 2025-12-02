using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrabalhoCapacitacao.Migrations
{
    public partial class AdicionarCamposODS11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AceitaRemoto",
                table: "Vagas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "Vagas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "EhVagaVerde",
                table: "Vagas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ZonaDaCidade",
                table: "Vagas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BairroResidencia",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "FocadoEmSustentabilidade",
                table: "Cursos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ImpactoComunitario",
                table: "Cursos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AceitaRemoto",
                table: "Vagas");

            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "Vagas");

            migrationBuilder.DropColumn(
                name: "EhVagaVerde",
                table: "Vagas");

            migrationBuilder.DropColumn(
                name: "ZonaDaCidade",
                table: "Vagas");

            migrationBuilder.DropColumn(
                name: "BairroResidencia",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "FocadoEmSustentabilidade",
                table: "Cursos");

            migrationBuilder.DropColumn(
                name: "ImpactoComunitario",
                table: "Cursos");
        }
    }
}
