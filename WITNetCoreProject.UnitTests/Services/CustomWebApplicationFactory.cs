using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WITNetCoreProject.Models.Entities;
using WITNetCoreProject.UnitTests.SharedDatabaseSetup;

namespace WITNetCoreProject.UnitTests.Services {

    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class {

        protected override void ConfigureWebHost(IWebHostBuilder builder) {

            builder.ConfigureServices(services => {

                // Remove the app's StoreContext registration.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<RepositoryContext>));

                if (descriptor != null) {

                    services.Remove(descriptor);
                }

                // Add StoreContext using an in-memory database for testing.
                services.AddDbContext<RepositoryContext>(options => {

                    options.UseInMemoryDatabase("InMemoryDbForFunctionalTesting");
                    options.EnableSensitiveDataLogging();
                });

                //services.AddDbContext<RepositoryContext>(opt => opt.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()), ServiceLifetime.Scoped, ServiceLifetime.Scoped);

                // Get service provider.
                var serviceProvider = services.BuildServiceProvider();

                using (var scope = serviceProvider.CreateScope()) {

                    var scopedServices = scope.ServiceProvider;

                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    var userDbContext = scopedServices.GetRequiredService<RepositoryContext>();
                    userDbContext.Database.EnsureCreated();

                    try {

                        DatabaseSetup.SeedData(userDbContext);
                    }
                    catch (Exception ex) {

                        logger.LogError(ex, $"An error occurred seeding the Store database with test messages. Error: {ex.Message}");
                    }
                }
            });
        }

        // this is custom configure service which implement like startup.cs in test environment
        public void CustomConfigureServices(IWebHostBuilder builder) {

            builder.ConfigureServices(services => {

                // Get service provider.
                var serviceProvider = services.BuildServiceProvider();

                using (var scope = serviceProvider.CreateScope()) {

                    var scopedServices = scope.ServiceProvider;

                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    var userDbContext = scopedServices.GetRequiredService<RepositoryContext>();

                    try {

                        DatabaseSetup.SeedData(userDbContext);
                    }
                    catch (Exception ex) {

                        logger.LogError(ex, $"An error occurred seeding the Store database with test messages. Error: {ex.Message}");
                    }
                }
            });
        }
    }
}
