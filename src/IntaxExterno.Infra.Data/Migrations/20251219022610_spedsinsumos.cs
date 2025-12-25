using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IntaxExterno.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class spedsinsumos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InsumosResultados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OportunidadeId = table.Column<int>(type: "integer", nullable: false),
                    DataApuracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DescricaoVerba = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CodigoCta = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ValorBase = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    ValorPis = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    ValorCofins = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    ValorTotal = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    UID = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    DeletedBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsumosResultados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsumosResultados_Oportunidades_OportunidadeId",
                        column: x => x.OportunidadeId,
                        principalTable: "Oportunidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SpedContabilI050",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OportunidadeId = table.Column<int>(type: "integer", nullable: false),
                    DataInicial = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataFinal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CodigoCta = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NomeCta = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Selecionado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    UID = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    DeletedBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpedContabilI050", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpedContabilI050_Oportunidades_OportunidadeId",
                        column: x => x.OportunidadeId,
                        principalTable: "Oportunidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SpedContabilI250",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OportunidadeId = table.Column<int>(type: "integer", nullable: false),
                    CodigoCta = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DataApuracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Descricao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Valor = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Selecionado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    UID = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    DeletedBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpedContabilI250", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpedContabilI250_Oportunidades_OportunidadeId",
                        column: x => x.OportunidadeId,
                        principalTable: "Oportunidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InsumosResultado_CodigoCta_OportunidadeId",
                table: "InsumosResultados",
                columns: new[] { "CodigoCta", "OportunidadeId" });

            migrationBuilder.CreateIndex(
                name: "IX_InsumosResultado_IsActive",
                table: "InsumosResultados",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_InsumosResultado_OportunidadeId_IsActive",
                table: "InsumosResultados",
                columns: new[] { "OportunidadeId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_InsumosResultado_UID",
                table: "InsumosResultados",
                column: "UID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpedContabilI050_IsActive",
                table: "SpedContabilI050",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_SpedContabilI050_OportunidadeId_CodigoCta",
                table: "SpedContabilI050",
                columns: new[] { "OportunidadeId", "CodigoCta" });

            migrationBuilder.CreateIndex(
                name: "IX_SpedContabilI050_OportunidadeId_IsActive",
                table: "SpedContabilI050",
                columns: new[] { "OportunidadeId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_SpedContabilI050_UID",
                table: "SpedContabilI050",
                column: "UID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpedContabilI250_IsActive",
                table: "SpedContabilI250",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_SpedContabilI250_OportunidadeId_CodigoCta_Selecionado",
                table: "SpedContabilI250",
                columns: new[] { "OportunidadeId", "CodigoCta", "Selecionado" });

            migrationBuilder.CreateIndex(
                name: "IX_SpedContabilI250_OportunidadeId_IsActive",
                table: "SpedContabilI250",
                columns: new[] { "OportunidadeId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_SpedContabilI250_UID",
                table: "SpedContabilI250",
                column: "UID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsumosResultados");

            migrationBuilder.DropTable(
                name: "SpedContabilI050");

            migrationBuilder.DropTable(
                name: "SpedContabilI250");
        }
    }
}
