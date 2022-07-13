using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WITNetCoreProject.Controllers;
using WITNetCoreProject.Models.Dtos;
using WITNetCoreProject.Models.Entities;
using WITNetCoreProject.Models.Mappings;
using WITNetCoreProject.Services.Interfaces;
using WITNetCoreProject.Services.Logger;
using WITNetCoreProject.UnitTests.Services;
using WITNetCoreProject.Utilities;
using Xunit;

namespace WITNetCoreProject.UnitTests.UnitTests
{
    public class UserApiControllerTests : IClassFixture<AppInstance>
    {
        private readonly AppInstance _instance;

        public UserApiControllerTests(AppInstance instance)
        {
            _instance = instance;
        }

        // this is command to perform unit test what is data type, what is response code status, or what is controller name return form one controller
        #region snippet_UserApiControllerTests
        [Fact]
        public async Task GetAllUsers_ReturnType_OK()
        {
            //Arrange
            var client = _instance
                .AuthenticatedInstance(new Claim("id", "6bdf4a32-fc3a-489f-1f8b-08da366b3141"),
                    new Claim(ClaimTypes.Name, "Mohammad Taufik Andrian"),
                    new Claim(ClaimTypes.Email, "taufikandrian18@gmail.com"))
                .CreateClient(new()
                {
                    AllowAutoRedirect = false,
                });

            //Act
            //var result = await controller.GetUsers();
            var response = await client.GetAsync("/api/UserApi/GetUsers");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseModel>(stringResponse);

            //Assert
            Assert.IsType<JArray>(result.Data);

        }
        #endregion
    }
}
