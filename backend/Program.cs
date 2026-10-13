using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
var origins = (builder.Configuration["FrontendOrigins"] ?? "http://localhost:5173").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
builder.Services.AddCors(o => o.AddPolicy("Frontend", p => p.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod()));
var connection = builder.Configuration.GetConnectionString("Default");
if (builder.Configuration.GetValue<bool>("RequireDatabase") && string.IsNullOrWhiteSpace(connection))
    throw new InvalidOperationException("ConnectionStrings__Default is required for this deployment.");
if (!string.IsNullOrWhiteSpace(connection)) builder.Services.AddDbContext<MenuDb>(o => o.UseNpgsql(connection));
var app = builder.Build();
app.UseCors("Frontend");
app.Use(async (context, next) => { await next(); app.Logger.LogInformation("{Method} {Path} responded {Status}", context.Request.Method, context.Request.Path, context.Response.StatusCode); });
app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "CampusEats.Api" }));
app.MapGet("/health/ready", async (IServiceProvider services) => {
    if (string.IsNullOrWhiteSpace(connection)) return Results.Ok(new { status = "ok", dataSource = "demo" });
    try {
        var db = services.GetRequiredService<MenuDb>();
        var count = await db.MenuItems.CountAsync();
        return Results.Ok(new { status = "ok", dataSource = "postgresql", menuItems = count });
    } catch { return Results.Problem("Database not ready.", statusCode: 503); }
});
app.MapGet("/api/menu", async (IServiceProvider services) => {
    if (string.IsNullOrWhiteSpace(connection)) return Results.Ok(Seed.Items);
    try { var db = services.GetRequiredService<MenuDb>(); return Results.Ok(await db.MenuItems.AsNoTracking().OrderBy(x => x.Id).ToListAsync()); }
    catch (Exception ex) { app.Logger.LogError(ex, "Database menu query failed"); return Results.Problem("The menu is temporarily unavailable.", statusCode: 503); }
});
app.Run();
public record MenuItem(int Id, string Name, string Category, decimal Price, string Description, string Emoji, string Tag);
public class MenuDb(DbContextOptions<MenuDb> options) : DbContext(options) {
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    protected override void OnModelCreating(ModelBuilder model) { model.Entity<MenuItem>().HasKey(x => x.Id); model.Entity<MenuItem>().Property(x => x.Price).HasPrecision(10, 2); model.Entity<MenuItem>().HasData(Seed.Items); }
}
public static class Seed { public static readonly MenuItem[] Items = [
new(1,"Chicken rice bowl","Meals",650,"Grilled chicken, fragrant rice and a bright garden salad.","🍛","Campus favourite"),
new(2,"Veggie power bowl","Meals",520,"Roasted vegetables, chickpeas and fresh greens.","🥗","Vegetarian"),
new(3,"Classic chicken burger","Snacks",480,"A toasted bun, crispy chicken and our house sauce.","🍔","Quick bite"),
new(4,"Cheese and tomato toast","Snacks",320,"Golden toast with melted cheese and ripe tomatoes.","🥪","Vegetarian"),
new(5,"Iced lime cooler","Drinks",220,"Fresh lime, a little sweetness and plenty of ice.","🍋","Refreshing"),
new(6,"Cold brew coffee","Drinks",350,"Smooth, slow-brewed coffee for your next lecture.","☕","Study fuel")]; }
