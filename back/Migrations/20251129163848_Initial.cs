using System;
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
            migrationBuilder.CreateTable(
                name: "Deposito",
                columns: table => new
                {
                    DepositoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre_deposito = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha_creacion = table.Column<DateOnly>(type: "date", nullable: false),
                    ZonaUtm = table.Column<decimal>(type: "Decimal(10,5)", nullable: false),
                    CoordenadaEste = table.Column<decimal>(type: "Decimal(10,5)", nullable: false),
                    CoordenadaNorte = table.Column<decimal>(type: "Decimal(10,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deposito", x => x.DepositoId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HashPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.IdUser);
                });

            migrationBuilder.CreateTable(
                name: "Hito",
                columns: table => new
                {
                    HitoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre_hito = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepositoId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "Piezometro",
                columns: table => new
                {
                    PiezometroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre_piezometro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Este = table.Column<decimal>(type: "Decimal(15,5)", nullable: false),
                    Norte = table.Column<decimal>(type: "Decimal(15,5)", nullable: false),
                    Elevacion = table.Column<decimal>(type: "Decimal(15,5)", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stick_up = table.Column<decimal>(type: "Decimal(10,5)", nullable: false),
                    Cota_actual_boca_tubo = table.Column<decimal>(type: "Decimal(10,5)", nullable: false),
                    Cota_actual_terreno = table.Column<decimal>(type: "Decimal(10,5)", nullable: false),
                    Cota_fondo_pozo = table.Column<decimal>(type: "Decimal(10,5)", nullable: false),
                    Profundidad_actual_pozo = table.Column<decimal>(type: "Decimal(10,5)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha_instalacion = table.Column<DateOnly>(type: "date", nullable: false),
                    DepositoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Piezometro", x => x.PiezometroId);
                    table.ForeignKey(
                        name: "FK_Piezometro_Deposito_DepositoId",
                        column: x => x.DepositoId,
                        principalTable: "Deposito",
                        principalColumn: "DepositoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicionHito",
                columns: table => new
                {
                    MedicionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    EsBase = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicionHito", x => x.MedicionId);
                    table.ForeignKey(
                        name: "FK_MedicionHito_Hito_HitoId",
                        column: x => x.HitoId,
                        principalTable: "Hito",
                        principalColumn: "HitoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicionPiezometro",
                columns: table => new
                {
                    MedicionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cota_Nivel_Piezometro = table.Column<decimal>(type: "Decimal(10,5)", nullable: false),
                    Longitud_medicion = table.Column<decimal>(type: "Decimal(10,5)", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha_medicion = table.Column<DateOnly>(type: "date", nullable: false),
                    PiezometroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicionPiezometro", x => x.MedicionId);
                    table.ForeignKey(
                        name: "FK_MedicionPiezometro_Piezometro_PiezometroId",
                        column: x => x.PiezometroId,
                        principalTable: "Piezometro",
                        principalColumn: "PiezometroId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hito_DepositoId",
                table: "Hito",
                column: "DepositoId");

            migrationBuilder.CreateIndex(
                name: "IX_Hito_Nombre_hito",
                table: "Hito",
                column: "Nombre_hito",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicionHito_HitoId",
                table: "MedicionHito",
                column: "HitoId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicionPiezometro_PiezometroId",
                table: "MedicionPiezometro",
                column: "PiezometroId");

            migrationBuilder.CreateIndex(
                name: "IX_Piezometro_DepositoId",
                table: "Piezometro",
                column: "DepositoId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicionHito");

            migrationBuilder.DropTable(
                name: "MedicionPiezometro");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Hito");

            migrationBuilder.DropTable(
                name: "Piezometro");

            migrationBuilder.DropTable(
                name: "Deposito");
        }
    }
}
