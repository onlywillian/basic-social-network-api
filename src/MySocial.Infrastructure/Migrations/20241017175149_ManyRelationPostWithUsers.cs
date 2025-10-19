using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySocial.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ManyRelationPostWithUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "profile_image",
                table: "Users",
                type: "VARCHAR",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 512);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "profile_image",
                table: "Users",
                type: "VARCHAR",
                maxLength: 512,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 512,
                oldNullable: true);
        }
    }
}
