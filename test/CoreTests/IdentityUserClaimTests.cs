namespace Tests
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Identity.DocumentDB;
    using Xunit;
    
    public class IdentityUserClaimTests
    {
        [Fact]
        public void Create_FromClaim_SetsTypeAndValue()
        {
            var claim = new Claim("type", "value");

            var userClaim = new IdentityClaim(claim);

            Assert.Equal("type", userClaim.Type);
            Assert.Equal("value", userClaim.Value);
        }

        [Fact]
        public void ToSecurityClaim_SetsTypeAndValue()
        {
            var userClaim = new IdentityClaim { Type = "t", Value = "v" };

            var claim = userClaim.ToSecurityClaim();

            Assert.Equal("t", claim.Type);
            Assert.Equal("v", claim.Value);
        }

        [Fact]
        public void ReplaceClaim_NoExistingClaim_Ignores()
        {
            // note: per EF implemention - only existing claims are updated by looping through them so that impl ignores too
            var user = new IdentityUser();
            var newClaim = new Claim("newType", "newValue");

            user.ReplaceClaim(newClaim, newClaim);

            Assert.Empty(user.Claims);
        }

        [Fact]
        public void ReplaceClaim_ExistingClaim_Replaces()
        {
            var user = new IdentityUser();
            var firstClaim = new Claim("type", "value");
            user.AddClaim(firstClaim);
            var newClaim = new Claim("newType", "newValue");

            user.ReplaceClaim(firstClaim, newClaim);

            user.ExpectOnlyHasThisClaim(newClaim);
        }

        [Fact]
        public void ReplaceClaim_ValueMatchesButTypeDoesNot_DoesNotReplace()
        {
            var user = new IdentityUser();
            var firstClaim = new Claim("type", "sameValue");
            user.AddClaim(firstClaim);
            var newClaim = new Claim("newType", "sameValue");

            user.ReplaceClaim(new Claim("wrongType", "sameValue"), newClaim);

            user.ExpectOnlyHasThisClaim(firstClaim);
        }

        [Fact]
        public void ReplaceClaim_TypeMatchesButValueDoesNot_DoesNotReplace()
        {
            var user = new IdentityUser();
            var firstClaim = new Claim("sameType", "value");
            user.AddClaim(firstClaim);
            var newClaim = new Claim("sameType", "newValue");

            user.ReplaceClaim(new Claim("sameType", "wrongValue"), newClaim);

            user.ExpectOnlyHasThisClaim(firstClaim);
        }
    }
}