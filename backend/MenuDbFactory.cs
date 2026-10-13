using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
public class MenuDbFactory : IDesignTimeDbContextFactory<MenuDb> {
    public MenuDb CreateDbContext(string[] args) {
        var connection = Environment.GetEnvironmentVariable("ConnectionStrings__Default") ?? "Host=localhost;Database=campuseats;Username=postgres";
        return new MenuDb(new DbContextOptionsBuilder<MenuDb>().UseNpgsql(connection).Options);
    }
}
