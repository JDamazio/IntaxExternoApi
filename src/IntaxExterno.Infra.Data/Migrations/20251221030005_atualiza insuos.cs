using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IntaxExterno.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class atualizainsuos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SpedContabilI250_OportunidadeId_CodigoCta_Selecionado",
                table: "SpedContabilI250");

            migrationBuilder.DropColumn(
                name: "Selecionado",
                table: "SpedContabilI250");

            migrationBuilder.DropColumn(
                name: "Selecionado",
                table: "SpedContabilI050");

            migrationBuilder.AddColumn<string>(
                name: "IndicadorDC",
                table: "SpedContabilI250",
                type: "character varying(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Situacao",
                table: "SpedContabilI250",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CodNatureza",
                table: "SpedContabilI050",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QtdI250",
                table: "SpedContabilI050",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QtdI250Selecionados",
                table: "SpedContabilI050",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "SpedContabilI050",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SpedContabilI155",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OportunidadeId = table.Column<int>(type: "integer", nullable: false),
                    CodCta = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    CodCcus = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    ValorDebito = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    ValorCredito = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    IndicadorSituacao = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_SpedContabilI155", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpedContabilI155_Oportunidades_OportunidadeId",
                        column: x => x.OportunidadeId,
                        principalTable: "Oportunidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpedContabilI250_OportunidadeId_CodigoCta_Situacao",
                table: "SpedContabilI250",
                columns: new[] { "OportunidadeId", "CodigoCta", "Situacao" });

            migrationBuilder.CreateIndex(
                name: "IX_SpedContabilI250_OportunidadeId_DataApuracao",
                table: "SpedContabilI250",
                columns: new[] { "OportunidadeId", "DataApuracao" });

            migrationBuilder.CreateIndex(
                name: "IX_SpedContabilI050_OportunidadeId_CodNatureza",
                table: "SpedContabilI050",
                columns: new[] { "OportunidadeId", "CodNatureza" });

            migrationBuilder.CreateIndex(
                name: "IX_SpedContabilI050_OportunidadeId_Status",
                table: "SpedContabilI050",
                columns: new[] { "OportunidadeId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_SpedContabilI155_OportunidadeId",
                table: "SpedContabilI155",
                column: "OportunidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_SpedContabilI155_OportunidadeId_CodCta",
                table: "SpedContabilI155",
                columns: new[] { "OportunidadeId", "CodCta" });

            migrationBuilder.CreateIndex(
                name: "IX_SpedContabilI155_OportunidadeId_DataInicio_DataFim",
                table: "SpedContabilI155",
                columns: new[] { "OportunidadeId", "DataInicio", "DataFim" });

            migrationBuilder.CreateIndex(
                name: "IX_SpedContabilI155_OportunidadeId_IndicadorSituacao",
                table: "SpedContabilI155",
                columns: new[] { "OportunidadeId", "IndicadorSituacao" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpedContabilI155");

            migrationBuilder.DropIndex(
                name: "IX_SpedContabilI250_OportunidadeId_CodigoCta_Situacao",
                table: "SpedContabilI250");

            migrationBuilder.DropIndex(
                name: "IX_SpedContabilI250_OportunidadeId_DataApuracao",
                table: "SpedContabilI250");

            migrationBuilder.DropIndex(
                name: "IX_SpedContabilI050_OportunidadeId_CodNatureza",
                table: "SpedContabilI050");

            migrationBuilder.DropIndex(
                name: "IX_SpedContabilI050_OportunidadeId_Status",
                table: "SpedContabilI050");

            migrationBuilder.DropColumn(
                name: "IndicadorDC",
                table: "SpedContabilI250");

            migrationBuilder.DropColumn(
                name: "Situacao",
                table: "SpedContabilI250");

            migrationBuilder.DropColumn(
                name: "CodNatureza",
                table: "SpedContabilI050");

            migrationBuilder.DropColumn(
                name: "QtdI250",
                table: "SpedContabilI050");

            migrationBuilder.DropColumn(
                name: "QtdI250Selecionados",
                table: "SpedContabilI050");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SpedContabilI050");

            migrationBuilder.AddColumn<bool>(
                name: "Selecionado",
                table: "SpedContabilI250",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Selecionado",
                table: "SpedContabilI050",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_SpedContabilI250_OportunidadeId_CodigoCta_Selecionado",
                table: "SpedContabilI250",
                columns: new[] { "OportunidadeId", "CodigoCta", "Selecionado" });
        }
    }
}
