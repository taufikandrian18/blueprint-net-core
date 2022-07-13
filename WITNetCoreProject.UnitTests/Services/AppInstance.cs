using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WITNetCoreProject.UnitTests.Services {

    public class AppInstance : WebApplicationFactory<Startup> {

        //dependencies Injection to startup class from main project

        public WebApplicationFactory<Startup> AuthenticatedInstance(params Claim[] claimSeed) {

            return WithWebHostBuilder(builder => {

                builder.ConfigureTestServices(services => {

                    //this is how to register a test scheme in this instance
                    services.AddSingleton<IAuthenticationSchemeProvider, MockSchemeProvider>();
                    services.AddSingleton<MockClaimSeed>(_ => new(claimSeed));
                });
            });
        }
    }

    //this class is show how to replicate authentication and authorization scheme in test environment
    public class MockSchemeProvider : AuthenticationSchemeProvider {

        public MockSchemeProvider(IOptions<AuthenticationOptions> options) : base(options) {
        }

        protected MockSchemeProvider(

            IOptions<AuthenticationOptions> options,
            IDictionary<string, AuthenticationScheme> schemes
        ) : base(options, schemes) {
        }

        public override Task<AuthenticationScheme> GetSchemeAsync(string name) {

            AuthenticationScheme mockScheme = new(
                JwtBearerDefaults.AuthenticationScheme,
                JwtBearerDefaults.AuthenticationScheme,
                typeof(MockAuthenticationHandler)
            );
            return Task.FromResult(mockScheme);
        }
    }

    //this class is how jwt bearer token provided and how is token to be handled by the instance in test environment scheme
    public class MockAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly MockClaimSeed _claimSeed;

        public MockAuthenticationHandler(
            MockClaimSeed claimSeed,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
        )
            : base(options, logger, encoder, clock)
        {
            _claimSeed = claimSeed;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claimsIdentity = new ClaimsIdentity(_claimSeed.getSeeds(), JwtBearerDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var ticket = new AuthenticationTicket(claimsPrincipal, JwtBearerDefaults.AuthenticationScheme);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }

    // this class is show you how to seed the mock token to be claimed and register to the instance
    public class MockClaimSeed
    {
        private readonly IEnumerable<Claim> _seed;

        public MockClaimSeed(IEnumerable<Claim> seed)
        {
            _seed = seed;
        }

        public IEnumerable<Claim> getSeeds() => _seed;
    }
}
