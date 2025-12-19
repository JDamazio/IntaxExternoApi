using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntaxExterno.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class tetagrande : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExclusaoIcmsResultados_Oportunidades_OportunidadeId",
                table: "ExclusaoIcmsResultados");

            migrationBuilder.DropIndex(
                name: "IX_ExclusaoIcmsResultados_OportunidadeId",
                table: "ExclusaoIcmsResultados");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorPisCofins",
                table: "ExclusaoIcmsResultados",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorPis",
                table: "ExclusaoIcmsResultados",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorIcms",
                table: "ExclusaoIcmsResultados",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorCofins",
                table: "ExclusaoIcmsResultados",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "ExclusaoIcmsResultados",
                type: "character varying(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UID",
                table: "ExclusaoIcmsResultados",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ExclusaoIcmsResultados",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "ExclusaoIcmsResultados",
                type: "character varying(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "ExclusaoIcmsResultados",
                type: "character varying(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OportunidadeId1",
                table: "ExclusaoIcmsResultados",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExclusaoIcmsResultado_IsActive",
                table: "ExclusaoIcmsResultados",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ExclusaoIcmsResultado_OportunidadeId_IsActive",
                table: "ExclusaoIcmsResultados",
                columns: new[] { "OportunidadeId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_ExclusaoIcmsResultado_UID",
                table: "ExclusaoIcmsResultados",
                column: "UID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExclusaoIcmsResultados_OportunidadeId1",
                table: "ExclusaoIcmsResultados",
                column: "OportunidadeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ExclusaoIcmsResultados_Oportunidades_OportunidadeId",
                table: "ExclusaoIcmsResultados",
                column: "OportunidadeId",
                principalTable: "Oportunidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExclusaoIcmsResultados_Oportunidades_OportunidadeId1",
                table: "ExclusaoIcmsResultados",
                column: "OportunidadeId1",
                principalTable: "Oportunidades",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExclusaoIcmsResultados_Oportunidades_OportunidadeId",
                table: "ExclusaoIcmsResultados");

            migrationBuilder.DropForeignKey(
                name: "FK_ExclusaoIcmsResultados_Oportunidades_OportunidadeId1",
                table: "ExclusaoIcmsResultados");

            migrationBuilder.DropIndex(
                name: "IX_ExclusaoIcmsResultado_IsActive",
                table: "ExclusaoIcmsResultados");

            migrationBuilder.DropIndex(
                name: "IX_ExclusaoIcmsResultado_OportunidadeId_IsActive",
                table: "ExclusaoIcmsResultados");

            migrationBuilder.DropIndex(
                name: "IX_ExclusaoIcmsResultado_UID",
                table: "ExclusaoIcmsResultados");

            migrationBuilder.DropIndex(
                name: "IX_ExclusaoIcmsResultados_OportunidadeId1",
                table: "ExclusaoIcmsResultados");

            migrationBuilder.DropColumn(
                name: "OportunidadeId1",
                table: "ExclusaoIcmsResultados");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorPisCofins",
                table: "ExclusaoIcmsResultados",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorPis",
                table: "ExclusaoIcmsResultados",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorIcms",
                table: "ExclusaoIcmsResultados",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorCofins",
                table: "ExclusaoIcmsResultados",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "ExclusaoIcmsResultados",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UID",
                table: "ExclusaoIcmsResultados",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(36)",
                oldMaxLength: 36);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ExclusaoIcmsResultados",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "ExclusaoIcmsResultados",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "ExclusaoIcmsResultados",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExclusaoIcmsResultados_OportunidadeId",
                table: "ExclusaoIcmsResultados",
                column: "OportunidadeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExclusaoIcmsResultados_Oportunidades_OportunidadeId",
                table: "ExclusaoIcmsResultados",
                column: "OportunidadeId",
                principalTable: "Oportunidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
