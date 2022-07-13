using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WITNetCoreProject.UnitTests.Services;
using WITNetCoreProject.Utilities;
using Xunit;

namespace WITNetCoreProject.UnitTests.Controllers {

    public class UserApiControllerTests : IClassFixture<AppInstance> {

        private readonly AppInstance _instance;

        public UserApiControllerTests(AppInstance instance) {

            _instance = instance;
        }

        /// <summary>
        /// integration test that show you whether this controller is succeed or not, however you have to passed the authorization first otherwise it would not be valid / (401 Authorized)
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetUsers_InvalidScope_ReturnsUnauthorizedResult() {

            //Arrange
            var client = _instance.CreateClient();
            var response = await client.GetAsync("/api/UserApi/GetUsers");

            //Act
            var expected = HttpStatusCode.Unauthorized;

            //Assert
            Assert.Equal(expected, response.StatusCode);
        }

        [Fact]
        public async Task GetUsers_ReturnsAllResult() {

            //Arrange
            var client = _instance
                .AuthenticatedInstance(new Claim("id", "1f207e51-ad52-4c55-a365-08da3c2334f4"),
                    new Claim(ClaimTypes.Name, "Mohammad Taufik Andrian"),
                    new Claim(ClaimTypes.Email, "taufikandrian18@gmail.com"))
                .CreateClient(new() {

                    AllowAutoRedirect = false,
                });

            var response = await client.GetAsync("/api/UserApi/GetUsers");
            response.EnsureSuccessStatusCode();

            //Act
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseModel>(stringResponse);
            var statusCode = response.StatusCode.ToString();

            //Assert
            Assert.Equal("OK", statusCode);
            Assert.True(result.Count == 3);
        }
    }
}
