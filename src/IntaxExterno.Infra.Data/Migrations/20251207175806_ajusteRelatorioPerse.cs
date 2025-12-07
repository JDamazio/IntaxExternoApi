using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntaxExterno.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class ajusteRelatorioPerse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnoPeriodo",
                table: "RelatoriosDeCreditoPerse");

            migrationBuilder.DropColumn(
                name: "MesPeriodo",
                table: "RelatoriosDeCreditoPerse");

            migrationBuilder.DropColumn(
                name: "AnoPeriodo",
                table: "ItensRelatorioDeCreditoPerse");

            migrationBuilder.DropColumn(
                name: "MesPeriodo",
                table: "ItensRelatorioDeCreditoPerse");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataEmissao",
                table: "RelatoriosDeCreditoPerse",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataEmissao",
                table: "ItensRelatorioDeCreditoPerse",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataEmissao",
                table: "RelatoriosDeCreditoPerse");

            migrationBuilder.DropColumn(
                name: "DataEmissao",
                table: "ItensRelatorioDeCreditoPerse");

            migrationBuilder.AddColumn<int>(
                name: "AnoPeriodo",
                table: "RelatoriosDeCreditoPerse",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MesPeriodo",
                table: "RelatoriosDeCreditoPerse",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AnoPeriodo",
                table: "ItensRelatorioDeCreditoPerse",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MesPeriodo",
                table: "ItensRelatorioDeCreditoPerse",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
