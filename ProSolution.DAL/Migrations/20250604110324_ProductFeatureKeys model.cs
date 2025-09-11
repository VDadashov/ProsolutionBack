using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProSolution.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ProductFeatureKeysmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductFeatureKeys",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFeatureKeys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductFeatureKeys_Catagories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Catagories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryFeatureOptions",
                columns: table => new
                {
                    ProductFeatureKeysId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FeatureOptionId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryFeatureOptions", x => new { x.ProductFeatureKeysId, x.FeatureOptionId });
                    table.ForeignKey(
                        name: "FK_CategoryFeatureOptions_FeatureOptions_FeatureOptionId",
                        column: x => x.FeatureOptionId,
                        principalTable: "FeatureOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryFeatureOptions_ProductFeatureKeys_ProductFeatureKeysId",
                        column: x => x.ProductFeatureKeysId,
                        principalTable: "ProductFeatureKeys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryFeatureOptions_FeatureOptionId",
                table: "CategoryFeatureOptions",
                column: "FeatureOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatureKeys_CategoryId",
                table: "ProductFeatureKeys",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryFeatureOptions");

            migrationBuilder.DropTable(
                name: "ProductFeatureKeys");
        }
    }
}
