using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntaxExterno.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class sped : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpedContribuicoes_Oportunidades_OportunidadeId",
                table: "SpedContribuicoes");

            migrationBuilder.DropForeignKey(
                name: "FK_SpedFiscais_Oportunidades_OportunidadeId",
                table: "SpedFiscais");

            migrationBuilder.RenameIndex(
                name: "IX_SpedFiscais_OportunidadeId",
                table: "SpedFiscais",
                newName: "IX_SpedFiscal_OportunidadeId");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorPisCofins",
                table: "SpedFiscais",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorPis",
                table: "SpedFiscais",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorIcms",
                table: "SpedFiscais",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorCofins",
                table: "SpedFiscais",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CstIcms",
                table: "SpedFiscais",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Cfop",
                table: "SpedFiscais",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "AliqPis",
                table: "SpedFiscais",
                type: "numeric(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AliqCofins",
                table: "SpedFiscais",
                type: "numeric(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorPisCofins",
                table: "SpedContribuicoes",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorPis",
                table: "SpedContribuicoes",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorIcms",
                table: "SpedContribuicoes",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorCofins",
                table: "SpedContribuicoes",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Regime",
                table: "SpedContribuicoes",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CodSitPis",
                table: "SpedContribuicoes",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CodFiscal",
                table: "SpedContribuicoes",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "AliqPis",
                table: "SpedContribuicoes",
                type: "numeric(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AliqCofins",
                table: "SpedContribuicoes",
                type: "numeric(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpedFiscal_DataInicial",
                table: "SpedFiscais",
                column: "DataInicial");

            migrationBuilder.CreateIndex(
                name: "IX_SpedFiscal_IsActive",
                table: "SpedFiscais",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_SpedContribuicoes_DataInicial",
                table: "SpedContribuicoes",
                column: "DataInicial");

            migrationBuilder.CreateIndex(
                name: "IX_SpedContribuicoes_IsActive",
                table: "SpedContribuicoes",
                column: "IsActive");

            migrationBuilder.AddForeignKey(
                name: "FK_SpedContribuicoes_Oportunidades_OportunidadeId",
                table: "SpedContribuicoes",
                column: "OportunidadeId",
                principalTable: "Oportunidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SpedFiscais_Oportunidades_OportunidadeId",
                table: "SpedFiscais",
                column: "OportunidadeId",
                principalTable: "Oportunidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpedContribuicoes_Oportunidades_OportunidadeId",
                table: "SpedContribuicoes");

            migrationBuilder.DropForeignKey(
                name: "FK_SpedFiscais_Oportunidades_OportunidadeId",
                table: "SpedFiscais");

            migrationBuilder.DropIndex(
                name: "IX_SpedFiscal_DataInicial",
                table: "SpedFiscais");

            migrationBuilder.DropIndex(
                name: "IX_SpedFiscal_IsActive",
                table: "SpedFiscais");

            migrationBuilder.DropIndex(
                name: "IX_SpedContribuicoes_DataInicial",
                table: "SpedContribuicoes");

            migrationBuilder.DropIndex(
                name: "IX_SpedContribuicoes_IsActive",
                table: "SpedContribuicoes");

            migrationBuilder.RenameIndex(
                name: "IX_SpedFiscal_OportunidadeId",
                table: "SpedFiscais",
                newName: "IX_SpedFiscais_OportunidadeId");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorPisCofins",
                table: "SpedFiscais",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorPis",
                table: "SpedFiscais",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorIcms",
                table: "SpedFiscais",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorCofins",
                table: "SpedFiscais",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CstIcms",
                table: "SpedFiscais",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Cfop",
                table: "SpedFiscais",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "AliqPis",
                table: "SpedFiscais",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AliqCofins",
                table: "SpedFiscais",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorPisCofins",
                table: "SpedContribuicoes",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorPis",
                table: "SpedContribuicoes",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorIcms",
                table: "SpedContribuicoes",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorCofins",
                table: "SpedContribuicoes",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Regime",
                table: "SpedContribuicoes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "CodSitPis",
                table: "SpedContribuicoes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "CodFiscal",
                table: "SpedContribuicoes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "AliqPis",
                table: "SpedContribuicoes",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AliqCofins",
                table: "SpedContribuicoes",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SpedContribuicoes_Oportunidades_OportunidadeId",
                table: "SpedContribuicoes",
                column: "OportunidadeId",
                principalTable: "Oportunidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpedFiscais_Oportunidades_OportunidadeId",
                table: "SpedFiscais",
                column: "OportunidadeId",
                principalTable: "Oportunidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
