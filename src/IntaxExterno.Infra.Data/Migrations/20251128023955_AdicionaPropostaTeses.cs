using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IntaxExterno.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaPropostaTeses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teses_Propostas_PropostaId",
                table: "Teses");

            migrationBuilder.DropIndex(
                name: "IX_Teses_PropostaId",
                table: "Teses");

            migrationBuilder.DropColumn(
                name: "PropostaId",
                table: "Teses");

            migrationBuilder.CreateTable(
                name: "PropostaTeses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PropostaId = table.Column<int>(type: "integer", nullable: false),
                    TesesId = table.Column<int>(type: "integer", nullable: false),
                    UID = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DeletedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaTeses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaTeses_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropostaTeses_Teses_TesesId",
                        column: x => x.TesesId,
                        principalTable: "Teses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropostaTeses_IsActive",
                table: "PropostaTeses",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaTeses_PropostaId",
                table: "PropostaTeses",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaTeses_PropostaId_TesesId",
                table: "PropostaTeses",
                columns: new[] { "PropostaId", "TesesId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropostaTeses_TesesId",
                table: "PropostaTeses",
                column: "TesesId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaTeses_UID",
                table: "PropostaTeses",
                column: "UID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropostaTeses");

            migrationBuilder.AddColumn<int>(
                name: "PropostaId",
                table: "Teses",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teses_PropostaId",
                table: "Teses",
                column: "PropostaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teses_Propostas_PropostaId",
                table: "Teses",
                column: "PropostaId",
                principalTable: "Propostas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
