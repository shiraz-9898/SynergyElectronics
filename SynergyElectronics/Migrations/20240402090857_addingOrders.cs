using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SynergyElectronics.Migrations
{
    /// <inheritdoc />
    public partial class addingOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Invoice_Id = table.Column<string>(type: "text", nullable: true),
                    Payment = table.Column<string>(type: "text", nullable: true),
                    Name_On_Card = table.Column<string>(type: "text", nullable: true),
                    Card_Num = table.Column<string>(type: "text", nullable: true),
                    Expiry = table.Column<string>(type: "text", nullable: true),
                    CVV = table.Column<int>(type: "integer", nullable: false),
                    Qty = table.Column<int>(type: "integer", nullable: false),
                    Price_Total = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Created_Date = table.Column<string>(type: "text", nullable: true),
                    User_Id = table.Column<string>(type: "text", nullable: false),
                    Prod_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_User_Id",
                        column: x => x.User_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Products_Prod_Id",
                        column: x => x.Prod_Id,
                        principalTable: "Products",
                        principalColumn: "Prod_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Prod_Id",
                table: "Orders",
                column: "Prod_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_User_Id",
                table: "Orders",
                column: "User_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
