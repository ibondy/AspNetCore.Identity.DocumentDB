namespace IntegrationTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.DocumentDB;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;
    
    public class EnsureWeCanExtendIdentityUserTests : UserIntegrationTestsBase
    {
        private UserManager<ExtendedIdentityUser> _Manager;
        private ExtendedIdentityUser _User;

        public class ExtendedIdentityUser : IdentityUser
        {
            public string ExtendedField { get; set; }
        }

        public EnsureWeCanExtendIdentityUserTests()
        {
            BeforeEachTestAfterBase();
        }

        public void BeforeEachTestAfterBase()
        {
            _Manager = CreateServiceProvider<ExtendedIdentityUser, IdentityRole>()
                .GetService<UserManager<ExtendedIdentityUser>>();
            _User = new ExtendedIdentityUser
            {
                UserName = "bob"
            };
        }

        [Fact]
        public async Task Create_ExtendedUserType_SavesExtraFields()
        {
            _User.ExtendedField = "extendedField";

            await _Manager.CreateAsync(_User);

            var savedUser = Client.CreateDocumentQuery<ExtendedIdentityUser>(Users.DocumentsLink).AsEnumerable().FirstOrDefault();
            Assert.Equal("extendedField", savedUser.ExtendedField);
        }

        [Fact]
        public async Task Create_ExtendedUserType_ReadsExtraFields()
        {
            _User.ExtendedField = "extendedField";

            await _Manager.CreateAsync(_User);

            var savedUser = await _Manager.FindByIdAsync(_User.Id);
            Assert.Equal("extendedField", savedUser.ExtendedField);
        }
    }
}