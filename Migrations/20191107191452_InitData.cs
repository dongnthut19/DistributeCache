using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DistributeCache.Migrations
{
    public partial class InitData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DistributeCache",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<byte[]>(nullable: true),
                    SlidingExpirationInSecond = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributeCache", x => x.Key);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DistributeCache");
        }
    }
}
