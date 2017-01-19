namespace IntegrationTests
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity.DocumentDB;
    using NUnit.Framework;
    using System.Security.Claims;

    // todo low - validate all tests work
    [TestFixture]
    public class UserRoleClaimStoreTests : UserIntegrationTestsBase
    {
        [Test]
        public async Task GetClaims_UserHasNoRoles_UserHasNoClaims_ReturnsNoClaims()
        {
            var manager = GetUserManager();
            var user = new IdentityUser { UserName = "bob" };
            await manager.CreateAsync(user);

            var claims = await manager.GetClaimsAsync(user);

            Expect(claims, Is.Empty);
        }

        [Test]
        public async Task GetClaims_UserHasRoles_UserHasNoRoleClaims_ReturnsNoClaims()
        {
            var manager = GetUserManager();
            var user = new IdentityUser { UserName = "bob" };
            await manager.CreateAsync(user);

            await manager.AddToRoleAsync(user, "role");

            var claims = await manager.GetClaimsAsync(user);

            Expect(claims, Is.Empty);
        }
    }
}