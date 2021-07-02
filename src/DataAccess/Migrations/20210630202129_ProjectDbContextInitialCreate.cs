using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class ProjectDbContextInitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryName", "CreatedBy", "CreatedDate", "LastModifiedBy", "LastModifiedDate", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("8d6af2ba-6234-4b2a-9448-1e299e2c4859"), "Phone", "Admin", new DateTime(2021, 6, 30, 23, 21, 28, 357, DateTimeKind.Local).AddTicks(2472), null, null, "Iphone 4", 400m },
                    { new Guid("82d9cb46-71c6-4c3f-982b-71983a0c740b"), "Phone", "Admin", new DateTime(2021, 6, 30, 23, 21, 28, 358, DateTimeKind.Local).AddTicks(4962), null, null, "Iphone 5", 400m },
                    { new Guid("34376e2c-9844-4d1d-9bbe-95d01353de9f"), "Phone", "Admin", new DateTime(2021, 6, 30, 23, 21, 28, 358, DateTimeKind.Local).AddTicks(4987), null, null, "Iphone 6", 400m },
                    { new Guid("254a0faf-281b-4aad-b7ac-d1e9d34d3df2"), "Phone", "Admin", new DateTime(2021, 6, 30, 23, 21, 28, 358, DateTimeKind.Local).AddTicks(4991), null, null, "Iphone 7", 400m },
                    { new Guid("2098ad11-2c36-447a-b94c-01603d3f1b33"), "Phone", "Admin", new DateTime(2021, 6, 30, 23, 21, 28, 358, DateTimeKind.Local).AddTicks(4994), null, null, "Iphone 8", 400m },
                    { new Guid("9f890060-76d6-4c69-a088-7655cbcf3ba7"), "Phone", "Admin", new DateTime(2021, 6, 30, 23, 21, 28, 358, DateTimeKind.Local).AddTicks(4998), null, null, "Iphone 10", 400m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
