namespace Tests
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Identity.DocumentDB;
    using Xunit;
    using Tests;

    public class IdentityRoleClaimTests
    {
        [Fact]
        public void Create_FromClaim_SetsTypeAndValue()
        {
            var claim = new Claim("type", "value");

            var roleClaim = new IdentityClaim(claim);

            Assert.Equal("type", roleClaim.Type);
            Assert.Equal("value", roleClaim.Value);
        }

        [Fact]
        public void ToSecurityClaim_SetsTypeAndValue()
        {
            var roleClaim = new IdentityClaim { Type = "t", Value = "v" };

            var claim = roleClaim.ToSecurityClaim();

            Assert.Equal("t", claim.Type);
            Assert.Equal("v", claim.Value);
        }

        [Fact]
        public void ReplaceClaim_NoExistingClaim_Ignores()
        {
            // note: per EF implemention - only existing claims are updated by looping through them so that impl ignores too
            var role = new IdentityRole();
            var newClaim = new Claim("newType", "newValue");

            role.ReplaceClaim(newClaim, newClaim);

            Assert.Empty(role.Claims);
        }

        [Fact]
        public void ReplaceClaim_ExistingClaim_Replaces()
        {
            var role = new IdentityRole();
            var firstClaim = new Claim("type", "value");
            role.AddClaim(firstClaim);
            var newClaim = new Claim("newType", "newValue");

            role.ReplaceClaim(firstClaim, newClaim);

            role.ExpectOnlyHasThisClaim(newClaim);
        }

        [Fact]
        public void ReplaceClaim_ValueMatchesButTypeDoesNot_DoesNotReplace()
        {
            var role = new IdentityRole();
            var firstClaim = new Claim("type", "sameValue");
            role.AddClaim(firstClaim);
            var newClaim = new Claim("newType", "sameValue");

            role.ReplaceClaim(new Claim("wrongType", "sameValue"), newClaim);

            role.ExpectOnlyHasThisClaim(firstClaim);
        }

        [Fact]
        public void ReplaceClaim_TypeMatchesButValueDoesNot_DoesNotReplace()
        {
            var role = new IdentityRole();
            var firstClaim = new Claim("sameType", "value");
            role.AddClaim(firstClaim);
            var newClaim = new Claim("sameType", "newValue");

            role.ReplaceClaim(new Claim("sameType", "wrongValue"), newClaim);

            role.ExpectOnlyHasThisClaim(firstClaim);
        }
    }
}