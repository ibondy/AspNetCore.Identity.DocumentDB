namespace Tests
{
	using Microsoft.AspNetCore.Identity.DocumentDB;
	using NUnit.Framework;

	// todo low - validate all tests work
	[TestFixture]
	public class IdentityRoleTests : AssertionHelper
	{
        /*
		[Test]
		public void ToBsonDocument_IdAssigned_MapsToBsonObjectId()
		{
			var role = new IdentityRole();

			var document = role.ToBsonDocument();

			Expect(document["_id"], Is.TypeOf<BsonObjectId>());
		}
        */

		[Test]
		public void Create_WithoutRoleName_HasIdAssigned()
		{
			var role = new IdentityRole();
            Expect(role, Is.Not.Null);
            Expect(role.Id, Is.Not.Null);
        }

		[Test]
		public void Create_WithRoleName_SetsName()
		{
			var name = "admin";

			var role = new IdentityRole(name);

			Expect(role.Name, Is.EqualTo(name));
		}

		[Test]
		public void Create_WithRoleName_SetsId()
		{
			var role = new IdentityRole("admin");
			Expect(role, Is.Not.Null);
			Expect(role.Id, Is.Not.Null);
		}
	}
}