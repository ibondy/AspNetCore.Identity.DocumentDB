namespace IntegrationTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity.DocumentDB;
    using Xunit;
    
    public class UserSecurityStampStoreTests : UserIntegrationTestsBase
    {
        [Fact]
        public async Task Create_NewUser_HasSecurityStamp()
        {
            var manager = GetUserManager();
            var user = new IdentityUser { UserName = "bob" };

            await manager.CreateAsync(user);

            var savedUser = Client.CreateDocumentQuery<IdentityUser>(Users.DocumentsLink).AsEnumerable().FirstOrDefault();
            Assert.NotNull(savedUser.SecurityStamp);
        }

        [Fact]
        public async Task GetSecurityStamp_NewUser_ReturnsStamp()
        {
            var manager = GetUserManager();
            var user = new IdentityUser { UserName = "bob" };
            await manager.CreateAsync(user);

            var stamp = await manager.GetSecurityStampAsync(user);

            Assert.NotNull(stamp);
        }
    }
}