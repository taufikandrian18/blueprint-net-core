using System;
using System.Net.Http;
using WITNetCoreProject.UnitTests.Services;
using Xunit;

namespace WITNetCoreProject.UnitTests.Controllers {

    public class BaseControllerTests : IClassFixture<CustomWebApplicationFactory<Program>> {

        // this class is for making a new test environment
        private readonly CustomWebApplicationFactory<Program> _factory;

        public BaseControllerTests(CustomWebApplicationFactory<Program> factory) {

            _factory = factory;
        }

        // this method will make a new object http client to call a controller API
        public HttpClient GetNewClient() {

            var newClient = _factory.WithWebHostBuilder(builder => {

                _factory.CustomConfigureServices(builder);
            }).CreateClient();

            return newClient;
        }
    }
}
