namespace Tests
{
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Identity.DocumentDB;
    using Xunit;

    public static class TestExtensions
    {
        public static void ExpectOnlyHasThisClaim(this IdentityUser user, Claim expectedClaim)
        {
            Assert.Single(user.Claims);
            var actualClaim = user.Claims.Single();
            Assert.Equal(expectedClaim.Type, actualClaim.Type);
            Assert.Equal(expectedClaim.Value, actualClaim.Value);
        }
    }
}