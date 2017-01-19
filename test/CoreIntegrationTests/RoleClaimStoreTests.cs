namespace IntegrationTests
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity.DocumentDB;
    using NUnit.Framework;

    [TestFixture]
    public class RoleClaimStoreTests : UserIntegrationTestsBase
    {
        [Test]
        public async Task Create_NewRole_HasNoClaims()
        {
            var role = new IdentityRole { Name = "name" };
            var manager = GetRoleManager();
            await manager.CreateAsync(role);

            var claims = await manager.GetClaimsAsync(role);

            Expect(claims, Is.Empty);
        }

        [Test]
        public async Task AddClaim_ReturnsClaim()
        {
            var role = new IdentityRole { Name = "name" };
            var manager = GetRoleManager();
            await manager.CreateAsync(role);

            await manager.AddClaimAsync(role, new Claim("type", "value"));

            var claim = (await manager.GetClaimsAsync(role)).Single();
            Expect(claim.Type, Is.EqualTo("type"));
            Expect(claim.Value, Is.EqualTo("value"));
        }

        [Test]
        public async Task RemoveClaim_RemovesExistingClaim()
        {
            var role = new IdentityRole { Name = "name" };
            var manager = GetRoleManager();
            await manager.CreateAsync(role);
            await manager.AddClaimAsync(role, new Claim("type", "value"));

            await manager.RemoveClaimAsync(role, new Claim("type", "value"));

            Expect(await manager.GetClaimsAsync(role), Is.Empty);
        }

        [Test]
        public async Task RemoveClaim_DifferentType_DoesNotRemoveClaim()
        {
            var role = new IdentityRole { Name = "name" };
            var manager = GetRoleManager();
            await manager.CreateAsync(role);
            await manager.AddClaimAsync(role, new Claim("type", "value"));

            await manager.RemoveClaimAsync(role, new Claim("otherType", "value"));

            Expect(await manager.GetClaimsAsync(role), Is.Not.Empty);
        }

        [Test]
        public async Task RemoveClaim_DifferentValue_DoesNotRemoveClaim()
        {
            var role = new IdentityRole { Name = "name" };
            var manager = GetRoleManager();
            await manager.CreateAsync(role);
            await manager.AddClaimAsync(role, new Claim("type", "value"));

            await manager.RemoveClaimAsync(role, new Claim("type", "otherValue"));

            Expect(await manager.GetClaimsAsync(role), Is.Not.Empty);
        }
    }
}