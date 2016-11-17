namespace Tests
{
	using Microsoft.AspNetCore.Identity.DocumentDB;
	using NUnit.Framework;

	// todo low - validate all tests work
	[TestFixture]
	public class IdentityRoleTests : AssertionHelper
	{
		[Test]
		public void Create_WithRoleName_SetsName()
		{
			var name = "admin";

			var role = new IdentityRole(name);

			Expect(role.Name, Is.EqualTo(name));
		}
	}
}