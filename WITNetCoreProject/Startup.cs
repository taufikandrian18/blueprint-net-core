using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using WITNetCoreProject.Extensions;
using WITNetCoreProject.Models.Authentications;

namespace WITNetCoreProject {

    public class Startup {

        public Startup(IConfiguration configuration) {

            // this command read the nlog.config directory
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            //ATTENTION : Configuration order has an impact to the application itself

            // this configuration lies in extensions/serviceExtension file
            services.ConfigureMySqlContext(Configuration);
            services.ConfigureCors();
            ////////////////////////////////////////////

            // this is configuration used when user need cookie to keep temp data
            services.Configure<CookiePolicyOptions>(options => {

                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.HttpOnly = HttpOnlyPolicy.Always;
                options.Secure = CookieSecurePolicy.Always;
            });

            // this is configuration used when user need cache to keep temp data
            services.AddDistributedMemoryCache();

            services.AddSession(options => {

                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            // this is configuration used when system use IIS server and the client need big capacity to ask request
            services.Configure<IISServerOptions>(options => {

                options.MaxRequestBodySize = int.MaxValue;
            });

            // this is configuration used when system use kestrel server and the client need big capacity to ask request
            services.Configure<KestrelServerOptions>(options => {

                options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB
            });

            // this configuration lies in extensions/serviceExtension file
            services.ConfigureIISIntegration();
            services.ConfigureLoggerService();
            services.ConfigureProjectServices();
            services.ConfigureRepositoryWrapper();
            services.AddSwaggerGen();
            services.ConfigureSwaggerGen();
            ////////////////////////////////////////////

            // this is configuration to declare automapper to the middleware
            services.AddAutoMapper(typeof(Startup));

            //this is configuration for token jwt
            AuthenticationConfiguration authenticationConfiguration = new AuthenticationConfiguration();
            Configuration.Bind("Authentication", authenticationConfiguration);

            services.AddSingleton(authenticationConfiguration);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(config => {

                    config.TokenValidationParameters = new TokenValidationParameters() {

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.AccessTokenSecret)),
                        ValidIssuer = authenticationConfiguration.Issuer,
                        ValidAudience = authenticationConfiguration.Audience,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ClockSkew = TimeSpan.Zero
                    };
                })
                .AddCookie("Cookies", options => {

                    options.Cookie.IsEssential = true;
                    options.Cookie.Name = "LoginAuthentication";
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Events = new CookieAuthenticationEvents {

                        OnRedirectToLogin = redirectContext => {

                            redirectContext.HttpContext.Response.StatusCode = 401;
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options => {

                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme);

                defaultAuthorizationPolicyBuilder =
                    defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();

                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });
            /////////////////////////////////////

            services.AddControllers()
               .AddNewtonsoftJson(options => {

                   // Use the default property (Pascal) casing
                   options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                   options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
               });
            services.AddMvc()
                .AddMvcOptions(option => {

                    option.EnableEndpointRouting = false;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddSessionStateTempDataProvider()
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {

            if (env.IsDevelopment()) {

                app.UseDeveloperExceptionPage();
            }
            else {

                app.UseExceptionHandler("/error"); //front-end have to provide /error page for this
            }

            //app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseForwardedHeaders(new ForwardedHeadersOptions {

                // this is contains all the headers parameters so we dont put the http headers manually
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => {

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WIT Backbone Project API");
            });

            app.UseCors("Development"); //Development configuration already declare in service extension file

            app.UseSession();
            app.UseRouting();

            app.UseCookiePolicy(
                new CookiePolicyOptions {

                    MinimumSameSitePolicy = SameSiteMode.None,
                    Secure = CookieSecurePolicy.Always
                });

            //the order should UseAuthentication first then UseAuthorization otherwise it would affect the application itself
            app.UseAuthentication();

            app.UseAuthorization();

            // this command is to make configuration exceptionHandler run programmatically
            app.UseExceptionHandler(c => c.Run(async context => {

                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;
                var response = new { error = exception.Message };
                await context.Response.WriteAsync(response.ToString(), cancellationToken: default);
            }));

            app.UseMvc();
            app.UseEndpoints(endpoints => {

                endpoints.MapDefaultControllerRoute();

                // this command used when the application want swagger as a landing page for the first time
                endpoints.MapGet("/", context => {

                    return Task.Run(() => context.Response.Redirect("/swagger/index.html"));
                });
            });
        }
    }
}
