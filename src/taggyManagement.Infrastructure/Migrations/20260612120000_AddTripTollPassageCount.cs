using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using taggyManagement.Infrastructure.Data;

#nullable disable

namespace taggyManagement.Infrastructure.Migrations;

[DbContext(typeof(TaggyDbContext))]
[Migration("20260612120000_AddTripTollPassageCount")]
public sealed class AddTripTollPassageCount : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "TollPassageCount",
            table: "Trips",
            type: "INTEGER",
            nullable: false,
            defaultValue: 0);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "TollPassageCount",
            table: "Trips");
    }
}
