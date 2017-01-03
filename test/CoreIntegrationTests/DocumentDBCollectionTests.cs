namespace IntegrationTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.DocumentDB;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;

    [TestFixture]
    public class DocumentDBCollectionTests : UserIntegrationTestsBase
    {
        [Test]
        public async Task CollectionWithMultipleDocumentTypes_Works()
        {
            // creates user and role and checks that they do not interfere with eachother
            // as both are stored in the same DocumentDB collection

            // create user
            var userName = "admin";
            var user = new IdentityUser { UserName = userName };
            var userManager = GetUserManager();

            await userManager.CreateAsync(user);

            // create role
            var roleName = "admin";
            var role = new IdentityRole(roleName);
            var roleManager = GetRoleManager();

            await roleManager.CreateAsync(role);

            // check query on collection
            var results = Client.CreateDocumentQuery(Users.DocumentsLink).ToList();
            Expect(results.Count, Is.EqualTo(2));

            // .NET Identity Framework - get all results
            var userList = userManager.Users.ToList();
            Expect(userList.Count, Is.EqualTo(1));
            Expect(userList[0].Id, Is.EqualTo(user.Id));

            var roleList = roleManager.Roles.ToList();
            Expect(roleList.Count, Is.EqualTo(1));
            Expect(roleList[0].Id, Is.EqualTo(role.Id));

            // .NET Identity Framework - get specific result
            var foundUser = await userManager.FindByNameAsync(userName);
            Expect(foundUser.UserName, Is.EqualTo(userName));
            Expect(foundUser.Id, Is.EqualTo(user.Id));

            var foundRole = await roleManager.FindByNameAsync(roleName);
            Expect(foundRole.Name, Is.EqualTo(roleName));
            Expect(foundRole.Id, Is.EqualTo(role.Id));

            Expect(foundUser.Id, Is.Not.EqualTo(foundRole.Id));
        }
    }
}
