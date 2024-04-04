using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SynergyElectronics.Migrations
{
    /// <inheritdoc />
    public partial class workingOncheckbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isSelected",
                table: "Carts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isSelected",
                table: "Carts");
        }
    }
}
