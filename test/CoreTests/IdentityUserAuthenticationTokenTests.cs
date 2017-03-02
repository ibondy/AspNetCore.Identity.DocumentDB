namespace CoreTests
{
    using Microsoft.AspNetCore.Identity.DocumentDB;
    using Xunit;

    public class IdentityUserAuthenticationTokenTests
    {
        [Fact]
        public void GetToken_NoTokens_ReturnsNull()
        {
            var user = new IdentityUser();

            var value = user.GetTokenValue("loginProvider", "tokenName");

            Assert.Null(value);
        }

        [Fact]
        public void GetToken_WithToken_ReturnsValueIfProviderAndNameMatch()
        {
            var user = new IdentityUser();
            user.SetToken("loginProvider", "tokenName", "tokenValue");

            Assert.Equal("tokenValue", user.GetTokenValue("loginProvider", "tokenName")); // "GetToken should match on both provider and name, but isn't"

            Assert.Null(user.GetTokenValue("wrongProvider", "tokenName")); // "GetToken should match on loginProvider, but isn't"

            Assert.Null(user.GetTokenValue("loginProvider", "wrongName")); // "GetToken should match on tokenName, but isn't"
        }

        [Fact]
        public void RemoveToken_OnlyRemovesIfNameAndProviderMatch()
        {
            var user = new IdentityUser();
            user.SetToken("loginProvider", "tokenName", "tokenValue");

            user.RemoveToken("wrongProvider", "tokenName");
            Assert.Equal("tokenValue", user.GetTokenValue("loginProvider", "tokenName")); // "RemoveToken should match on loginProvider, but isn't"

            user.RemoveToken("loginProvider", "wrongName");
            Assert.Equal("tokenValue", user.GetTokenValue("loginProvider", "tokenName")); // "RemoveToken should match on tokenName, but isn't"

            user.RemoveToken("loginProvider", "tokenName");
            Assert.Null(user.GetTokenValue("loginProvider", "tokenName")); // "RemoveToken should match on both loginProvider and tokenName, but isn't"
        }

        [Fact]
        public void SetToken_ReplacesValue()
        {
            var user = new IdentityUser();
            user.SetToken("loginProvider", "tokenName", "tokenValue");

            user.SetToken("loginProvider", "tokenName", "updatedValue");

            Assert.Equal(1, user.Tokens.Count);
            Assert.Equal("updatedValue", user.GetTokenValue("loginProvider", "tokenName"));
        }
    }
}