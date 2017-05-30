namespace CoreIntegrationTests
{
    using System.Threading.Tasks;
    using IntegrationTests;
    using Microsoft.AspNetCore.Identity.DocumentDB;
    using Xunit;

    public class UserAuthenticationTokenStoreTests : UserIntegrationTestsBase
    {
        [Fact]
        public async Task SetGetAndRemoveTokens()
        {
            // note: this is just an integration test, testing of IdentityUser behavior is in domain/unit tests
            var user = new IdentityUser() { UserName = "test" };
            var manager = GetUserManager();
            await manager.CreateAsync(user);

            await manager.SetAuthenticationTokenAsync(user, "loginProvider", "tokenName", "tokenValue");

            var tokenValue = await manager.GetAuthenticationTokenAsync(user, "loginProvider", "tokenName");
            Assert.Equal("tokenValue", tokenValue);

            await manager.RemoveAuthenticationTokenAsync(user, "loginProvider", "tokenName");
            var afterRemovedValue = await manager.GetAuthenticationTokenAsync(user, "loginProvider", "tokenName");
            Assert.Null(afterRemovedValue);
        }
    }
}