using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SynergyElectronics.Migrations
{
    /// <inheritdoc />
    public partial class addOtherTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Category_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category_Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Category_Id);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    SubCategory_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubCategory_Name = table.Column<string>(type: "text", nullable: true),
                    Category_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.SubCategory_Id);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_Category_Id",
                        column: x => x.Category_Id,
                        principalTable: "Categories",
                        principalColumn: "Category_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Prod_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Prod_Img = table.Column<string>(type: "text", nullable: true),
                    Prod_Name = table.Column<string>(type: "text", nullable: true),
                    Prod_Desc = table.Column<string>(type: "text", nullable: true),
                    Prod_Price = table.Column<double>(type: "numeric(10,2)", nullable: false),
                    SubCategory_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Prod_Id);
                    table.ForeignKey(
                        name: "FK_Products_SubCategories_SubCategory_Id",
                        column: x => x.SubCategory_Id,
                        principalTable: "SubCategories",
                        principalColumn: "SubCategory_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Cart_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cart_Qty = table.Column<int>(type: "integer", nullable: false),
                    Cart_Price = table.Column<double>(type: "numeric(10,2)", nullable: false),
                    User_Id = table.Column<string>(type: "text", nullable: false),
                    Prod_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Cart_Id);
                    table.ForeignKey(
                        name: "FK_Carts_AspNetUsers_User_Id",
                        column: x => x.User_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carts_Products_Prod_Id",
                        column: x => x.Prod_Id,
                        principalTable: "Products",
                        principalColumn: "Prod_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_Prod_Id",
                table: "Carts",
                column: "Prod_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_User_Id",
                table: "Carts",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubCategory_Id",
                table: "Products",
                column: "SubCategory_Id");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_Category_Id",
                table: "SubCategories",
                column: "Category_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
