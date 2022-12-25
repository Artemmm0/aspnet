using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class CustomEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    Price = table.Column<double>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 40, nullable: false),
                    Age = table.Column<int>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "CompanyId", "Name", "Price" },
                values: new object[] { new Guid("aad4c053-49b6-410c-bc78-2d54a9991870"), new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "cool name", 1773.0 });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "CompanyId", "Name", "Price" },
                values: new object[] { new Guid("abd4c053-49b6-410c-bc78-2d54a9991870"), new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "untitled", 300.0 });

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "Id", "Age", "Name", "ProductId" },
                values: new object[] { new Guid("f9d4c053-49b6-410c-bc78-2d54a9991870"), 28, "Sachin Woolley", new Guid("abd4c053-49b6-410c-bc78-2d54a9991870") });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ProductId",
                table: "Customer",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CompanyId",
                table: "Product",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
