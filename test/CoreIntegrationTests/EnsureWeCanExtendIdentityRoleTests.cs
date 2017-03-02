namespace IntegrationTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.DocumentDB;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;
    
    public class EnsureWeCanExtendIdentityRoleTests : UserIntegrationTestsBase
    {
        private RoleManager<ExtendedIdentityRole> _Manager;
        private ExtendedIdentityRole _Role;

        public class ExtendedIdentityRole : IdentityRole
        {
            public string ExtendedField { get; set; }
        }

        public EnsureWeCanExtendIdentityRoleTests()
        {
            BeforeEachTestAfterBase();
        }

        public void BeforeEachTestAfterBase()
        {
            _Manager = CreateServiceProvider<IdentityUser, ExtendedIdentityRole>()
                .GetService<RoleManager<ExtendedIdentityRole>>();
            _Role = new ExtendedIdentityRole
            {
                Name = "admin"
            };
        }

        [Fact]
        public async Task Create_ExtendedRoleType_SavesExtraFields()
        {
            _Role.ExtendedField = "extendedField";

            await _Manager.CreateAsync(_Role);

            var savedRole = Client.CreateDocumentQuery<ExtendedIdentityRole>(Roles.DocumentsLink).ToList().FirstOrDefault();
            Assert.Equal("extendedField", savedRole.ExtendedField);
        }

        [Fact]
        public async Task Create_ExtendedRoleType_ReadsExtraFields()
        {
            _Role.ExtendedField = "extendedField";

            await _Manager.CreateAsync(_Role);

            var savedRole = await _Manager.FindByIdAsync(_Role.Id);
            Assert.Equal("extendedField", savedRole.ExtendedField);
        }
    }
}