using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IntaxExterno.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class addRelatoriDeCreditoPerse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RelatoriosDeCreditoPerse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnoPeriodo = table.Column<int>(type: "integer", nullable: false),
                    TotalIRPJ = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalCSLL = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalPIS = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalCOFINS = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Saldo = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ClienteId = table.Column<int>(type: "integer", nullable: false),
                    UID = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DeletedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatoriosDeCreditoPerse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelatoriosDeCreditoPerse_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItensRelatorioDeCreditoPerse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipoTributo = table.Column<string>(type: "text", nullable: false),
                    MesPeriodo = table.Column<int>(type: "integer", nullable: false),
                    AnoPeriodo = table.Column<int>(type: "integer", nullable: false),
                    NumPedido = table.Column<int>(type: "integer", nullable: true),
                    TotalSolicitado = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CorrecaoMonetaria = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    TotalRecebido = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Observacao = table.Column<string>(type: "text", nullable: true),
                    RelatorioDeCreditoPerseId = table.Column<int>(type: "integer", nullable: false),
                    UID = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DeletedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensRelatorioDeCreditoPerse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensRelatorioDeCreditoPerse_RelatoriosDeCreditoPerse_Relat~",
                        column: x => x.RelatorioDeCreditoPerseId,
                        principalTable: "RelatoriosDeCreditoPerse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItensRelatorioDeCreditoPerse_RelatorioDeCreditoPerseId",
                table: "ItensRelatorioDeCreditoPerse",
                column: "RelatorioDeCreditoPerseId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatoriosDeCreditoPerse_ClienteId",
                table: "RelatoriosDeCreditoPerse",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItensRelatorioDeCreditoPerse");

            migrationBuilder.DropTable(
                name: "RelatoriosDeCreditoPerse");
        }
    }
}
