using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using WITNetCoreProject.Models.Entities;
using WITNetCoreProject.Services.Interfaces;
using WITNetCoreProject.Services.Interfaces.Auth;
using WITNetCoreProject.Services.Logger;
using WITNetCoreProject.Services.Repositories;
using WITNetCoreProject.Services.Repositories.Auth;
using WITNetCoreProject.Utilities;

namespace WITNetCoreProject.Extensions {

    public static class ServiceExtensions {

        // this is configuration function for sql connection while the connection string lies in appsetting file
        public static void ConfigureMySqlContext(this IServiceCollection services, IConfiguration config) {

            var connectionString = config["sqlconnection:connectionString"];
            //services.AddDbContext<RepositoryContext>(o => o.UseSqlServer(connectionString).EnableSensitiveDataLogging());
            services.AddDbContext<RepositoryContext>(p => p.UseNpgsql(connectionString));
        }

        // this is configuration function for cross origin between this system which recieve or send request
        public static void ConfigureCors(this IServiceCollection services) {

            services.AddCors(options =>
                  options.AddPolicy("Development", builder =>
                  {
                                  // Allow multiple HTTP methods  
                                  builder.WithMethods("GET", "POST", "PATCH", "DELETE", "OPTIONS")
                                    .WithHeaders(
                                      HeaderNames.Accept,
                                      HeaderNames.SetCookie,
                                      HeaderNames.ContentType,
                                      HeaderNames.Authorization)
                                    .AllowCredentials()
                                    .SetIsOriginAllowed(origin =>
                                        {
                                            if (string.IsNullOrWhiteSpace(origin)) return false;
                                            if (origin.ToLower().StartsWith("http://localhost")) return true;
                                            return false;
                                        });
                  })
            );
        }

        // this is configuration function for IIS if you host your project inside IIS
        public static void ConfigureIISIntegration(this IServiceCollection services) {

            services.Configure<IISOptions>(options => {
            });
        }

        // this is configuration function for logging using Nlog, and you have to declare here for every repositories and interfaces related to log function
        public static void ConfigureLoggerService(this IServiceCollection services) {

            services.AddSingleton<ILoggerManager, LoggerManagerRepository>();
        }

        // this is configuration function for services in this project, and you have to declare here for every repositories and interfaces related to this project
        public static void ConfigureProjectServices(this IServiceCollection services) {

            services.AddScoped<TokenProjectServices>();
            services.AddSingleton<RefreshTokenGenerator>();
            services.AddSingleton<RefreshTokenValidator>();
            services.AddScoped<Authenticator>();
            services.AddSingleton<TokenGenerator>();
            services.AddScoped<IRefreshTokenRepository, DatabaseRefreshTokenRepository>();
            services.AddScoped<DatabaseRefreshTokenRepository>();
            //services.AddSingleton<IHostedService, ApacheKafkaConsumerService>();

        }

        // this is configuration function for repository wrapper in this project, and you have to declare here for repository wrapper related to this project
        public static void ConfigureRepositoryWrapper(this IServiceCollection services) {

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }

        // this is configuration function for swagger in this project, and you have to declare here for every configuration you want for swagger functionality
        public static void ConfigureSwaggerGen(this IServiceCollection services) {

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Version = "v1",
                    Title = "WIT Backbone Project API",
                    Description = "A Collections for swagger Backbone Project API information",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact {
                        Name = "Mohammad Taufik Andrian",
                        Email = "taufikandrian18@gmail.com",
                        Url = new Uri("https://example.com"),
                    },
                    License = new OpenApiLicense {
                        Name = "Use under OpenApiLicense",
                        Url = new Uri("https://example.com/license"),
                    }
                });
            });
        }
    }
}
