namespace Tests
{
    using Microsoft.AspNetCore.Identity.DocumentDB;
    using Xunit;

    // todo low - validate all tests work
    public class IdentityRoleTests
    {
        [Fact]
        public void Create_WithRoleName_SetsName()
        {
            var name = "admin";

            var role = new IdentityRole(name);

            Assert.Equal(name, role.Name);
        }
    }
}