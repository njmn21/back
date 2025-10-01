using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Deposito",
                columns: table => new
                {
                    DepositoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre_deposito = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ubicacion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fecha_creacion = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deposito", x => x.DepositoId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Hito",
                columns: table => new
                {
                    HitoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre_hito = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DepositoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hito", x => x.HitoId);
                    table.ForeignKey(
                        name: "FK_Hito_Deposito_DepositoId",
                        column: x => x.DepositoId,
                        principalTable: "Deposito",
                        principalColumn: "DepositoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Medicion",
                columns: table => new
                {
                    MedicionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Este = table.Column<decimal>(type: "decimal(15,5)", nullable: false),
                    Norte = table.Column<decimal>(type: "decimal(15,5)", nullable: false),
                    Elevacion = table.Column<decimal>(type: "decimal(15,5)", nullable: false),
                    HorizontalAbsoluto = table.Column<decimal>(type: "decimal(15,5)", nullable: false),
                    VerticalAbsoluto = table.Column<decimal>(type: "decimal(15,5)", nullable: false),
                    TotalAbsoluto = table.Column<decimal>(type: "decimal(15,5)", nullable: false),
                    AcimutAbsoluto = table.Column<decimal>(type: "decimal(15,5)", nullable: false),
                    BuzamientoAbsoluto = table.Column<decimal>(type: "decimal(15,5)", nullable: false),
                    HorizontalRelativo = table.Column<decimal>(type: "decimal(15,5)", nullable: false),
                    VerticalRelativo = table.Column<decimal>(type: "decimal(15,5)", nullable: false),
                    TotalRelativo = table.Column<decimal>(type: "decimal(15,5)", nullable: false),
                    AcimutRelativo = table.Column<decimal>(type: "decimal(15,5)", nullable: false),
                    BuzamientoRelativo = table.Column<decimal>(type: "decimal(15,5)", nullable: false),
                    HorizontalAcmulado = table.Column<decimal>(type: "decimal(15,5)", nullable: false),
                    VelocidadMedia = table.Column<decimal>(type: "decimal(15,5)", nullable: false),
                    InversaVelocidadMedia = table.Column<decimal>(type: "decimal(15,5)", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    FrecuenciaMonitoreo = table.Column<int>(type: "int", nullable: false),
                    TiempoDias = table.Column<int>(type: "int", nullable: false),
                    HitoId = table.Column<int>(type: "int", nullable: false),
                    EsBase = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicion", x => x.MedicionId);
                    table.ForeignKey(
                        name: "FK_Medicion_Hito_HitoId",
                        column: x => x.HitoId,
                        principalTable: "Hito",
                        principalColumn: "HitoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Hito_DepositoId",
                table: "Hito",
                column: "DepositoId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicion_HitoId",
                table: "Medicion",
                column: "HitoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medicion");

            migrationBuilder.DropTable(
                name: "Hito");

            migrationBuilder.DropTable(
                name: "Deposito");
        }
    }
}
