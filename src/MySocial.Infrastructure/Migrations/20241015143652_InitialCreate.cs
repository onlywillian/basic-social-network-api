using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySocial.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false),
                    nickname = table.Column<string>(type: "VARCHAR", maxLength: 50, nullable: false),
                    birthdate = table.Column<DateOnly>(type: "VARCHAR", maxLength: 8, nullable: false),
                    Cep = table.Column<string>(type: "TEXT", nullable: false),
                    password = table.Column<string>(type: "VARCHAR", maxLength: 128, nullable: false),
                    profile_image = table.Column<string>(type: "VARCHAR", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
