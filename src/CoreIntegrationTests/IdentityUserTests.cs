namespace IntegrationTests
{
	using Microsoft.AspNetCore.Identity.DocumentDB;
	using MongoDB.Bson;
	using NUnit.Framework;

	[TestFixture]
	public class IdentityUserTests : UserIntegrationTestsBase
	{
		[Test]
		public void Insert_NoId_SetsId()
		{
			var user = new IdentityUser();
			user.Id = null;

			Client.CreateDocumentAsync(Users.DocumentsLink, user);

			Expect(user, Is.Not.Null);
			Expect(user.Id, Is.Not.Null);
		}
	}
}