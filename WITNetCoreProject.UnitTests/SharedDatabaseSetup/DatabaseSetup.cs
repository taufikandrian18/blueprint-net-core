using System;
using Bogus;
using WITNetCoreProject.Models.Entities;
using WITNetCoreProject.Utilities;

namespace WITNetCoreProject.UnitTests.SharedDatabaseSetup {

    public class DatabaseSetup {

        public static void SeedData(RepositoryContext context) {

            context.Users.RemoveRange(context.Users);

            var fakeOwners = new Faker<Users>()
                .RuleFor(o => o.UserId, f => Guid.NewGuid())
                .RuleFor(o => o.Username, f => $"test")
                .RuleFor(o => o.Password, f => $"hfhjwhfwhqjfq832748")
                .RuleFor(o => o.DisplayName, f => $"fullnameTest")
                .RuleFor(o => o.Email, f => $"emailTest")
                .RuleFor(o => o.Phone, f => $"phoneTest")
                .RuleFor(o => o.IsDeleted, f => false)
                .RuleFor(o => o.CreatedBy, f => $"User")
                .RuleFor(o => o.UpdatedBy, f => $"User")
                .RuleFor(o => o.CreatedAt, f => DateUtil.GetCurrentDate())
                .RuleFor(o => o.UpdatedAt, f => DateUtil.GetCurrentDate());

            var owners = fakeOwners.Generate(10);

            context.AddRange(owners);

            context.SaveChanges();
        }
    }
}
