using Microsoft.EntityFrameworkCore;

namespace MyApi.Data;

public static class DatabaseConnection
{
    public static IServiceCollection AddDatabaseConnection(
        this IServiceCollection services)
    {
        string connectionString =
            "Server=localhost;Port=3306;Database=DonationDb;User=root;Password=123456789;";

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySQL(connectionString));

        return services;
    }
}