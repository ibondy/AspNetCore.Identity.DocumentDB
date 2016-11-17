namespace IntegrationTests
{
	using Microsoft.AspNetCore.Identity.DocumentDB;
	using NUnit.Framework;

	[TestFixture]
	public class IdentityUserTests : UserIntegrationTestsBase
	{
		[Test]
		public void Insert_NoId_SetsId()
		{
			var user = new IdentityUser();
			user.Id = null;

			user = (dynamic) Client.CreateDocumentAsync(Users.DocumentsLink, user).Result.Resource;

			Expect(user, Is.Not.Null);
			Expect(user.Id, Is.Not.Null);
		}
	}
}