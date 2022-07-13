using System;
using System.Linq;
using AutoMapper;
using WITNetCoreProject.Models.Mappings;
using WITNetCoreProject.Services.Repositories;
using WITNetCoreProject.UnitTests.SharedDatabaseSetup;
using Xunit;

namespace WITNetCoreProject.UnitTests.Repositories {

    public class UserRepositoryTests : IClassFixture<SharedDatabaseFixture> {

        private readonly IMapper _mapper;
        // declare how to use in memory database for functional test
        private SharedDatabaseFixture Fixture {

            get;
        }

        // constructor implement in memory database
        public UserRepositoryTests(SharedDatabaseFixture fixture) {

            Fixture = fixture;
            var configuration = new MapperConfiguration(cfg => {

                cfg.AddProfile<GeneralProfile>();
            });
            _mapper = configuration.CreateMapper();
        }

        /// <summary>
        /// perform function user repository test
        /// </summary>
        [Fact]
        public async void GetUsers_ReturnsAllUsers() {

            using (var context = Fixture.CreateContext()) {

                var repository = new UserRepository(context);
                var users = await repository.GetUsers();
                Assert.Equal(10, users.Count());
            }
        }
    }
}
