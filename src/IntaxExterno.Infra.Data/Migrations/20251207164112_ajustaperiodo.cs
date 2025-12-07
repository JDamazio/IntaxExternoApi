using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntaxExterno.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class ajustaperiodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MesPeriodo",
                table: "RelatoriosDeCreditoPerse",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MesPeriodo",
                table: "RelatoriosDeCreditoPerse");
        }
    }
}
