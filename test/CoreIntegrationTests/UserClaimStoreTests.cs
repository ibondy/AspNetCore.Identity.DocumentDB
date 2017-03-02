namespace IntegrationTests
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity.DocumentDB;
    using Xunit;
    
    public class UserClaimStoreTests : UserIntegrationTestsBase
    {
        [Fact]
        public async Task Create_NewUser_HasNoClaims()
        {
            var user = new IdentityUser { UserName = "bob" };
            var manager = GetUserManager();
            await manager.CreateAsync(user);

            var claims = await manager.GetClaimsAsync(user);

            Assert.Empty(claims);
        }

        [Fact]
        public async Task AddClaim_ReturnsClaim()
        {
            var user = new IdentityUser { UserName = "bob" };
            var manager = GetUserManager();
            await manager.CreateAsync(user);

            await manager.AddClaimAsync(user, new Claim("type", "value"));

            var claim = (await manager.GetClaimsAsync(user)).Single();
            Assert.Equal("type", claim.Type);
            Assert.Equal("value", claim.Value);
        }

        [Fact]
        public async Task RemoveClaim_RemovesExistingClaim()
        {
            var user = new IdentityUser { UserName = "bob" };
            var manager = GetUserManager();
            await manager.CreateAsync(user);
            await manager.AddClaimAsync(user, new Claim("type", "value"));

            await manager.RemoveClaimAsync(user, new Claim("type", "value"));

            Assert.Empty(await manager.GetClaimsAsync(user));
        }

        [Fact]
        public async Task RemoveClaim_DifferentType_DoesNotRemoveClaim()
        {
            var user = new IdentityUser { UserName = "bob" };
            var manager = GetUserManager();
            await manager.CreateAsync(user);
            await manager.AddClaimAsync(user, new Claim("type", "value"));

            await manager.RemoveClaimAsync(user, new Claim("otherType", "value"));

            Assert.NotEmpty(await manager.GetClaimsAsync(user));
        }

        [Fact]
        public async Task RemoveClaim_DifferentValue_DoesNotRemoveClaim()
        {
            var user = new IdentityUser { UserName = "bob" };
            var manager = GetUserManager();
            await manager.CreateAsync(user);
            await manager.AddClaimAsync(user, new Claim("type", "value"));

            await manager.RemoveClaimAsync(user, new Claim("type", "otherValue"));

            Assert.NotEmpty(await manager.GetClaimsAsync(user));
        }

        [Fact]
        public async Task ReplaceClaim_Replaces()
        {
            // note: unit tests cover behavior of ReplaceClaim method on IdentityUser
            var user = new IdentityUser { UserName = "bob" };
            var manager = GetUserManager();
            await manager.CreateAsync(user);
            var existingClaim = new Claim("type", "value");
            await manager.AddClaimAsync(user, existingClaim);
            var newClaim = new Claim("newType", "newValue");

            await manager.ReplaceClaimAsync(user, existingClaim, newClaim);

            // TODO
            // user.ExpectOnlyHasThisClaim(newClaim);
        }

        [Fact]
        public async Task GetUsersForClaim()
        {
            var userWithClaim = new IdentityUser
            {
                UserName = "with"
            };
            var userWithout = new IdentityUser();
            var manager = GetUserManager();
            await manager.CreateAsync(userWithClaim);
            await manager.CreateAsync(userWithout);
            var claim = new Claim("sameType", "sameValue");
            await manager.AddClaimAsync(userWithClaim, claim);

            var matchedUsers = await manager.GetUsersForClaimAsync(claim);

            Assert.Single(matchedUsers);
            Assert.Equal("with", matchedUsers.Single().UserName);

            var matchesForWrongType = await manager.GetUsersForClaimAsync(new Claim("wrongType", "sameValue"));
            Assert.Empty(matchesForWrongType); // "Users with claim with wrongType should not be returned but were."

            var matchesForWrongValue = await manager.GetUsersForClaimAsync(new Claim("sameType", "wrongValue"));
            Assert.Empty(matchesForWrongValue); // "Users with claim with wrongValue should not be returned but were."
        }
    }
}