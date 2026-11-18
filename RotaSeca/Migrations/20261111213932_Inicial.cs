using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RotaSeca.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    ModoPreferido = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LugaresSalvos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    Etiqueta = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Endereco = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    Dias = table.Column<int>(type: "INTEGER", nullable: false),
                    HorarioHabitual = table.Column<TimeOnly>(type: "TEXT", nullable: true),
                    UltimoRisco = table.Column<double>(type: "REAL", nullable: true),
                    RiscoAtualizadoEm = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LugaresSalvos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LugaresSalvos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reportes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: true),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    Nivel = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Comentario = table.Column<string>(type: "TEXT", maxLength: 280, nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Confirmacoes = table.Column<int>(type: "INTEGER", nullable: false),
                    ConfirmacoesDeBaixa = table.Column<int>(type: "INTEGER", nullable: false),
                    EncerradoEm = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reportes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reportes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Rotas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrigemLatitude = table.Column<double>(type: "REAL", nullable: false),
                    OrigemLongitude = table.Column<double>(type: "REAL", nullable: false),
                    DestinoLatitude = table.Column<double>(type: "REAL", nullable: false),
                    DestinoLongitude = table.Column<double>(type: "REAL", nullable: false),
                    Modo = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    OpcoesAvaliadas = table.Column<int>(type: "INTEGER", nullable: false),
                    OpcoesBloqueadas = table.Column<int>(type: "INTEGER", nullable: false),
                    SemCaminhoSeco = table.Column<bool>(type: "INTEGER", nullable: false),
                    DescricaoRecomendada = table.Column<string>(type: "TEXT", maxLength: 60, nullable: true),
                    DistanciaKm = table.Column<double>(type: "REAL", nullable: true),
                    DuracaoMinutos = table.Column<int>(type: "INTEGER", nullable: true),
                    RiscoMaximo = table.Column<double>(type: "REAL", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rotas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rotas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alertas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    LugarSalvoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Probabilidade = table.Column<double>(type: "REAL", nullable: false),
                    Nivel = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Mensagem = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Lido = table.Column<bool>(type: "INTEGER", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alertas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alertas_LugaresSalvos_LugarSalvoId",
                        column: x => x.LugarSalvoId,
                        principalTable: "LugaresSalvos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alertas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "AtualizadoEm", "CriadoEm", "ModoPreferido", "Nome" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "APe", "Usuária Teste A (simulada)" },
                    { 2, null, new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MotoBike", "Usuário Teste B (simulado)" }
                });

            migrationBuilder.InsertData(
                table: "LugaresSalvos",
                columns: new[] { "Id", "AtualizadoEm", "CriadoEm", "Dias", "Endereco", "Etiqueta", "HorarioHabitual", "Latitude", "Longitude", "RiscoAtualizadoEm", "UltimoRisco", "UsuarioId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 62, "Endereço simulado - Vale do Anhangabaú, Centro", "Trabalho", new TimeOnly(8, 30, 0), -23.545500000000001, -46.637, null, null, 1 },
                    { 2, null, new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 127, "Endereço simulado - Vila Mariana", "Casa", null, -23.588999999999999, -46.634, null, null, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_LugarSalvoId",
                table: "Alertas",
                column: "LugarSalvoId");

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_UsuarioId_Lido",
                table: "Alertas",
                columns: new[] { "UsuarioId", "Lido" });

            migrationBuilder.CreateIndex(
                name: "IX_LugaresSalvos_UltimoRisco",
                table: "LugaresSalvos",
                column: "UltimoRisco");

            migrationBuilder.CreateIndex(
                name: "IX_LugaresSalvos_UsuarioId_Etiqueta",
                table: "LugaresSalvos",
                columns: new[] { "UsuarioId", "Etiqueta" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reportes_Status_CriadoEm",
                table: "Reportes",
                columns: new[] { "Status", "CriadoEm" });

            migrationBuilder.CreateIndex(
                name: "IX_Reportes_UsuarioId",
                table: "Reportes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Rotas_UsuarioId_CriadoEm",
                table: "Rotas",
                columns: new[] { "UsuarioId", "CriadoEm" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alertas");

            migrationBuilder.DropTable(
                name: "Reportes");

            migrationBuilder.DropTable(
                name: "Rotas");

            migrationBuilder.DropTable(
                name: "LugaresSalvos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
