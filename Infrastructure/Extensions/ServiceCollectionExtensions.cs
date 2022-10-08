using Core.Settings;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AdddAndMigrateTenantDatabases(this IServiceCollection services,IConfiguration config)
        {
            var options =services.GetOptions<TenantSettings>(nameof(TenantSettings));
            var defaultConnectionString = options.Defaults?.ConnectionString;
            var defaultProvider = options.Defaults?.DBProvider;
            if(defaultProvider?.ToLower()== "mysql")
            {
                services.AddDbContext<DataContext>(m => m.UseSqlServer(e => e.MigrationsAssembly(typeof(DataContext).Assembly.FullName)));
            }
            var tenants = options.Tenants;
            foreach (var tenant in tenants)
            {
                string connectionString;
                if (string.IsNullOrEmpty(tenant.ConnectionString))
                {
                    connectionString = defaultConnectionString;
                }
                else
                {
                    connectionString=tenant.ConnectionString;
                }
                using var scope=services.BuildServiceProvider().CreateScope();
                var dbContext=scope.ServiceProvider.GetRequiredService<DataContext>();
                dbContext.Database.SetConnectionString(connectionString);
                if(dbContext.Database.GetMigrations().Count()> 0)
                {
                    dbContext.Database.Migrate();
                }

            }
            return services;
        }

        public static T GetOptions<T>(this IServiceCollection services,string sectionName) where T : new()
        {
            using var serviceProvider=services.BuildServiceProvider();  
            var configuration=serviceProvider.GetRequiredService<IConfiguration>();
            var section=configuration.GetSection(sectionName);
            var options = new T();
            section.Bind(options);
            return options;
        }
    }
}
