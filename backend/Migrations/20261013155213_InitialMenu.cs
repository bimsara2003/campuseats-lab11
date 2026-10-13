using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CampusEats.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Emoji = table.Column<string>(type: "text", nullable: false),
                    Tag = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Category", "Description", "Emoji", "Name", "Price", "Tag" },
                values: new object[,]
                {
                    { 1, "Meals", "Grilled chicken, fragrant rice and a bright garden salad.", "🍛", "Chicken rice bowl", 650m, "Campus favourite" },
                    { 2, "Meals", "Roasted vegetables, chickpeas and fresh greens.", "🥗", "Veggie power bowl", 520m, "Vegetarian" },
                    { 3, "Snacks", "A toasted bun, crispy chicken and our house sauce.", "🍔", "Classic chicken burger", 480m, "Quick bite" },
                    { 4, "Snacks", "Golden toast with melted cheese and ripe tomatoes.", "🥪", "Cheese and tomato toast", 320m, "Vegetarian" },
                    { 5, "Drinks", "Fresh lime, a little sweetness and plenty of ice.", "🍋", "Iced lime cooler", 220m, "Refreshing" },
                    { 6, "Drinks", "Smooth, slow-brewed coffee for your next lecture.", "☕", "Cold brew coffee", 350m, "Study fuel" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuItems");
        }
    }
}
