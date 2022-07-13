using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WITNetCoreProject.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(unicode: false, maxLength: 15, nullable: false),
                    Password = table.Column<string>(unicode: false, maxLength: 15, nullable: false),
                    DisplayName = table.Column<string>(unicode: false, maxLength: 64, nullable: false),
                    Phone = table.Column<string>(unicode: false, maxLength: 64, nullable: false),
                    Email = table.Column<string>(unicode: false, maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "TB_Refresh_Tokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<Guid>(nullable: false),
                    Token = table.Column<string>(unicode: false, maxLength: 2147483647, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Refresh_Tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users",
                        column: x => x.UserId,
                        principalTable: "TB_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_Refresh_Tokens_UserId",
                table: "TB_Refresh_Tokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_Refresh_Tokens");

            migrationBuilder.DropTable(
                name: "TB_Users");
        }
    }
}
