using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Novus_Top_Trumps.Migrations
{
    public partial class CarsCards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "CarsCard",
               columns: table => new
               {
                   ID = table.Column<int>(type: "int", nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   Speed = table.Column<int>(type: "int", nullable: false),
                   Horsepower = table.Column<int>(type: "int", nullable: false),
                   Weight = table.Column<int>(type: "int", nullable: false),
                   Price = table.Column<int>(type: "int", nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_CarsCard", x => x.ID);
               });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarsCard");
        }
    }
}
