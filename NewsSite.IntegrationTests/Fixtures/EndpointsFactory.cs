using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;

namespace NewsSite.IntegrationTests.Fixtures;

public class EndpointsFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _databaseContainer = new MsSqlBuilder().Build();

    public Task InitializeAsync()
    {
        return _databaseContainer.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _databaseContainer.StopAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<OnlineNewsContext>));
            services.AddDbContext<OnlineNewsContext>(options =>
                options.UseSqlServer(_databaseContainer.GetConnectionString()));
        });
    }
}