namespace Tests
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Identity.DocumentDB;
    using NUnit.Framework;

    [TestFixture]
    public class IdentityRoleClaimTests : AssertionHelper
    {
        [Test]
        public void Create_FromClaim_SetsTypeAndValue()
        {
            var claim = new Claim("type", "value");

            var roleClaim = new IdentityClaim(claim);

            Expect(roleClaim.Type, Is.EqualTo("type"));
            Expect(roleClaim.Value, Is.EqualTo("value"));
        }

        [Test]
        public void ToSecurityClaim_SetsTypeAndValue()
        {
            var roleClaim = new IdentityClaim { Type = "t", Value = "v" };

            var claim = roleClaim.ToSecurityClaim();

            Expect(claim.Type, Is.EqualTo("t"));
            Expect(claim.Value, Is.EqualTo("v"));
        }

        [Test]
        public void ReplaceClaim_NoExistingClaim_Ignores()
        {
            // note: per EF implemention - only existing claims are updated by looping through them so that impl ignores too
            var role = new IdentityRole();
            var newClaim = new Claim("newType", "newValue");

            role.ReplaceClaim(newClaim, newClaim);

            Expect(role.Claims, Is.Empty);
        }

        [Test]
        public void ReplaceClaim_ExistingClaim_Replaces()
        {
            var role = new IdentityRole();
            var firstClaim = new Claim("type", "value");
            role.AddClaim(firstClaim);
            var newClaim = new Claim("newType", "newValue");

            role.ReplaceClaim(firstClaim, newClaim);

            role.ExpectOnlyHasThisClaim(newClaim);
        }

        [Test]
        public void ReplaceClaim_ValueMatchesButTypeDoesNot_DoesNotReplace()
        {
            var role = new IdentityRole();
            var firstClaim = new Claim("type", "sameValue");
            role.AddClaim(firstClaim);
            var newClaim = new Claim("newType", "sameValue");

            role.ReplaceClaim(new Claim("wrongType", "sameValue"), newClaim);

            role.ExpectOnlyHasThisClaim(firstClaim);
        }

        [Test]
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