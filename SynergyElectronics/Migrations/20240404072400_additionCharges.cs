using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SynergyElectronics.Migrations
{
    /// <inheritdoc />
    public partial class additionCharges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Additional_Charges",
                table: "Orders",
                type: "numeric(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Additional_Charges",
                table: "Orders");
        }
    }
}
