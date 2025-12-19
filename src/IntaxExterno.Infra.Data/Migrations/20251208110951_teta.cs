using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IntaxExterno.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class teta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Oportunidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClienteId = table.Column<int>(type: "integer", nullable: false),
                    ParceiroId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioOrigemId = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataFechamento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Oportunidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Oportunidades_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Oportunidades_Parceiros_ParceiroId",
                        column: x => x.ParceiroId,
                        principalTable: "Parceiros",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExclusaoIcmsResultados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OportunidadeId = table.Column<int>(type: "integer", nullable: false),
                    DataInicial = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValorIcms = table.Column<decimal>(type: "numeric", nullable: true),
                    ValorPis = table.Column<decimal>(type: "numeric", nullable: true),
                    ValorCofins = table.Column<decimal>(type: "numeric", nullable: true),
                    ValorPisCofins = table.Column<decimal>(type: "numeric", nullable: true),
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
                    table.PrimaryKey("PK_ExclusaoIcmsResultados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExclusaoIcmsResultados_Oportunidades_OportunidadeId",
                        column: x => x.OportunidadeId,
                        principalTable: "Oportunidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OportunidadeTeses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OportunidadeId = table.Column<int>(type: "integer", nullable: false),
                    TesesId = table.Column<int>(type: "integer", nullable: false),
                    Observacoes = table.Column<string>(type: "text", nullable: true),
                    ValorEstimado = table.Column<decimal>(type: "numeric", nullable: true),
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
                    table.PrimaryKey("PK_OportunidadeTeses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OportunidadeTeses_Oportunidades_OportunidadeId",
                        column: x => x.OportunidadeId,
                        principalTable: "Oportunidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OportunidadeTeses_Teses_TesesId",
                        column: x => x.TesesId,
                        principalTable: "Teses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpedContribuicoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OportunidadeId = table.Column<int>(type: "integer", nullable: false),
                    CodFiscal = table.Column<string>(type: "text", nullable: false),
                    CodSitPis = table.Column<string>(type: "text", nullable: false),
                    AliqPis = table.Column<decimal>(type: "numeric", nullable: true),
                    AliqCofins = table.Column<decimal>(type: "numeric", nullable: true),
                    ValorIcms = table.Column<decimal>(type: "numeric", nullable: true),
                    ValorPis = table.Column<decimal>(type: "numeric", nullable: true),
                    ValorCofins = table.Column<decimal>(type: "numeric", nullable: true),
                    ValorPisCofins = table.Column<decimal>(type: "numeric", nullable: true),
                    DataInicial = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Regime = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_SpedContribuicoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpedContribuicoes_Oportunidades_OportunidadeId",
                        column: x => x.OportunidadeId,
                        principalTable: "Oportunidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpedFiscais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OportunidadeId = table.Column<int>(type: "integer", nullable: false),
                    Cfop = table.Column<string>(type: "text", nullable: false),
                    CstIcms = table.Column<string>(type: "text", nullable: false),
                    AliqPis = table.Column<decimal>(type: "numeric", nullable: true),
                    AliqCofins = table.Column<decimal>(type: "numeric", nullable: true),
                    ValorIcms = table.Column<decimal>(type: "numeric", nullable: true),
                    ValorPis = table.Column<decimal>(type: "numeric", nullable: true),
                    ValorCofins = table.Column<decimal>(type: "numeric", nullable: true),
                    ValorPisCofins = table.Column<decimal>(type: "numeric", nullable: true),
                    DataInicial = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_SpedFiscais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpedFiscais_Oportunidades_OportunidadeId",
                        column: x => x.OportunidadeId,
                        principalTable: "Oportunidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExclusaoIcmsResultados_OportunidadeId",
                table: "ExclusaoIcmsResultados",
                column: "OportunidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Oportunidades_ClienteId",
                table: "Oportunidades",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Oportunidades_ParceiroId",
                table: "Oportunidades",
                column: "ParceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_OportunidadeTeses_OportunidadeId",
                table: "OportunidadeTeses",
                column: "OportunidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_OportunidadeTeses_TesesId",
                table: "OportunidadeTeses",
                column: "TesesId");

            migrationBuilder.CreateIndex(
                name: "IX_SpedContribuicoes_OportunidadeId",
                table: "SpedContribuicoes",
                column: "OportunidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_SpedFiscais_OportunidadeId",
                table: "SpedFiscais",
                column: "OportunidadeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExclusaoIcmsResultados");

            migrationBuilder.DropTable(
                name: "OportunidadeTeses");

            migrationBuilder.DropTable(
                name: "SpedContribuicoes");

            migrationBuilder.DropTable(
                name: "SpedFiscais");

            migrationBuilder.DropTable(
                name: "Oportunidades");
        }
    }
}
